using API.Document.Domain;

namespace API.Document;

public static class OperationalTransformation
{
    public static DocumentAction Transform(DocumentAction action, List<DocumentAction> completedActions)
    {
        var docVersion = completedActions.Count;
        if (action.Revision == docVersion + 1) return action;
        if (action.Revision > docVersion + 1) throw new Exception("Invalid revision number");

        var transformedAction = completedActions[action.Revision..].Aggregate(action, TransformAction);
        if (transformedAction == null) throw new Exception("Invalid transformation");
        return transformedAction;
    }

    private static DocumentAction TransformAction(DocumentAction newAction, DocumentAction oldAction)
    {
        if (newAction.IsInsert() && oldAction.IsInsert()) return InsertOverInsert(newAction, oldAction);
        if (newAction.IsInsert() && oldAction.IsDelete()) return InsertOverDelete(newAction, oldAction);
        if (newAction.IsInsert() && oldAction.IsUpdate()) return InsertOverUpdate(newAction, oldAction);
        if (newAction.IsDelete() && oldAction.IsInsert()) return DeleteOverInsert(newAction, oldAction);
        if (newAction.IsDelete() && oldAction.IsDelete()) return DeleteOverDelete(newAction, oldAction);
        if (newAction.IsDelete() && oldAction.IsUpdate()) return DeleteOverUpdate(newAction, oldAction);
        if (newAction.IsUpdate() && oldAction.IsInsert()) return UpdateOverInsert(newAction, oldAction);
        if (newAction.IsUpdate() && oldAction.IsDelete()) return UpdateOverDelete(newAction, oldAction);
        if (newAction.IsUpdate() && oldAction.IsUpdate()) return UpdateOverUpdate(newAction, oldAction);
        throw new Exception("Unhandled condition. This should never happen.");
    }
    
    public static DocumentAction InsertOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        
        // new insert is before old insert => no change
        if (newAction.Position < oldAction.Position) return new DocumentAction(revision, newAction.Position, newAction.Insert, null, client);
        
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var newPosition = newAction.Position + oldInsertLength;
        // new insert is after old insert => offset new insert by old insert length
        return new DocumentAction(revision, newPosition, newAction.Insert, null, client);
    }
    
    public static DocumentAction DeleteOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var deleteLength = newAction.Delete ?? 0;
        var insertLength = oldAction.Insert?.Length ?? 0;
        var positionDelta = Math.Abs(newAction.Position - oldAction.Position);
            
        // delete is after insert => offset delete by insertion length
        if (newAction.Position >= oldAction.Position)
        {
            var newPosition = newAction.Position + insertLength;
            return new DocumentAction(revision, newPosition, null, deleteLength, client);
        }

        // delete is way before insert => no change
        if (positionDelta >= deleteLength)
            return new DocumentAction(revision, newAction.Position, null, deleteLength, client);
        
        var newDelete = deleteLength + insertLength;
        // delete is just before insert (conflict) => update with target character deletion and insert previous string
        return new DocumentAction(revision, newAction.Position, oldAction.Insert, newDelete, client);
    }
    
    public static DocumentAction UpdateOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var deleteLength = newAction.Delete ?? 0;
        var newInsertLength = newAction.Insert?.Length ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        
        // update is after insert => offset update by insertion length
        if (newAction.Position >= oldAction.Position)
        {
            var newPosition = newAction.Position + oldInsertLength;
            return new DocumentAction(revision, newPosition, newAction.Insert, deleteLength, client);
        }
        
        var positionDelta = oldAction.Position - newAction.Position;
        
        // update is way before insert => no change
        if (positionDelta >= deleteLength)
            return new DocumentAction(revision, newAction.Position, newAction.Insert, deleteLength, client);

        // update is just before insert (conflict) => update with target character deletion and insert string before previous string
        var newDelete = deleteLength + newInsertLength;
        var newInsert = (newAction.Insert ?? "") + (oldAction.Insert ?? "");
        return new DocumentAction(revision, newAction.Position, newInsert, newDelete, client);
    }
    
    public static DocumentAction InsertOverDelete(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var deleteLength = oldAction.Delete ?? 0;
        
        // new insert is before old delete => no change
        if (newAction.Position < oldAction.Position) return new DocumentAction(revision, newAction.Position, newAction.Insert, null, client);

        // new insert is after old delete => offset new insert by delete length
        var positionDelta = newAction.Position - oldAction.Position;
        var delta = positionDelta > deleteLength ? deleteLength : positionDelta;
        var newPosition = newAction.Position - delta;
        return new DocumentAction(revision, newPosition, newAction.Insert, null, client);
    }
    
    public static DocumentAction DeleteOverDelete(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;

        // new delete is before old delete => offset old delete by new delete length
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            // new delete is way before old delete => no change
            if (positionDelta > newDelete)
                return new DocumentAction(revision, newAction.Position, null, newDelete, client);

            // new delete is just before old delete (conflict) => delete extra characters
            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            return new DocumentAction(revision, newAction.Position, null, positionDelta + deleteRemainingOrZero, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        // new delete is way after old delete => offset new delete by old delete length
        if (positionDif > oldDelete) return new DocumentAction(revision, newAction.Position - oldDelete, null, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        // new delete is just after old delete (conflict) => delete extra characters
        return deleteSurplus <= 0 ? 
            new DocumentAction(revision, newAction.Position, null, 0, client)
            : new DocumentAction(revision, oldAction.Position, null, deleteSurplus, client);
    }

    public static DocumentAction UpdateOverDelete(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;
        
        // new update is before old delete
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            // new update is way before old delete => no change
            if (positionDelta > newDelete)
                return new DocumentAction(revision, newAction.Position, newAction.Insert, newAction.Delete, client);

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            
            // new update is just before old delete (conflict) => update, deleting excess characters 
            return new DocumentAction(revision, newAction.Position, newAction.Insert, positionDelta + deleteRemainingOrZero, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        // new update is way after old delete => offset new update by old delete length
        if (positionDif > oldDelete) 
            return new DocumentAction(revision, newAction.Position - oldDelete, newAction.Insert, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        // new update is just after old delete (conflict) => update from old position, deleting excess characters
        return deleteSurplus <= 0
            ? new DocumentAction(revision, oldAction.Position, newAction.Insert, 0, client)
            : new DocumentAction(revision, oldAction.Position, newAction.Insert, deleteSurplus, client);
    }

    public static DocumentAction InsertOverUpdate(DocumentAction newAction, DocumentAction oldAction)
    {   
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var oldDelete = oldAction.Delete ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var oldNetLength = oldInsertLength - oldDelete;
        
        // new insert is before old update => no change
        if (newAction.Position < oldAction.Position)
            return new DocumentAction(revision, newAction.Position, newAction.Insert, null, client);

        var positionDelta = newAction.Position - oldAction.Position;
        
        // new insert is after old update => offset new insert by old net length
        return positionDelta > oldDelete
            ? new DocumentAction(revision, newAction.Position + oldNetLength, newAction.Insert, null, client)
            : new DocumentAction(revision, newAction.Position + oldInsertLength - positionDelta, newAction.Insert, null, client);
    }

    public static DocumentAction DeleteOverUpdate(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var oldNetLength = oldInsertLength - oldDelete;
        
        // new delete is before old update
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            // new delete is way before old update => no change
            if (positionDelta > newDelete)
                return new DocumentAction(revision, newAction.Position, null, newAction.Delete, client);

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            // new delete is just before old update (conflict) => delete extra characters
            return new DocumentAction(revision, newAction.Position, oldAction.Insert, positionDelta + deleteRemainingOrZero + oldInsertLength, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        // new delete is way after old update => offset new delete by old net length
        if (positionDif > oldDelete) 
            return new DocumentAction(revision, newAction.Position + oldNetLength, null, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        // new delete is just after old update (conflict) => delete extra characters
        return deleteSurplus <= 0
            ? new DocumentAction(revision, newAction.Position, null, 0, client)
            : new DocumentAction(revision, newAction.Position, oldAction.Insert?[positionDif..] ?? "", oldInsertLength - positionDif + deleteSurplus, client);
    }

    public static DocumentAction UpdateOverUpdate(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var oldNetLength = oldInsertLength - oldDelete;
        
        // new update is before old update
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            // new update is way before old update => no change
            if (positionDelta > newDelete)
                return new DocumentAction(revision, newAction.Position, newAction.Insert, newAction.Delete, client);

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            // new update is just before old update (conflict) => update, deleting excess characters
            return new DocumentAction(revision, newAction.Position, newAction.Insert + (oldAction.Insert ?? ""), positionDelta + oldInsertLength + deleteRemainingOrZero, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        // new update is way after old update => offset new update by old net length
        if (positionDif > oldDelete) 
            return new DocumentAction(revision, newAction.Position + oldNetLength, newAction.Insert, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        var deleteSurplusOrZero = deleteSurplus < 0 ? 0 : deleteSurplus;
        // new update is just after old update (conflict) => update from old position, deleting excess characters
        return new DocumentAction(revision, oldAction.Position + oldInsertLength, newAction.Insert, deleteSurplusOrZero, client);
    }
}