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
        
        if (newAction.Position < oldAction.Position) return new DocumentAction(revision, newAction.Position, newAction.Insert, null, client);
        
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var newPosition = newAction.Position + oldInsertLength;
        return new DocumentAction(revision, newPosition, newAction.Insert, null, client);
    }
    
    public static DocumentAction DeleteOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var deleteLength = newAction.Delete ?? 0;
        var insertLength = oldAction.Insert?.Length ?? 0;
            
        if (newAction.Position >= oldAction.Position)
        {

            var newPosition = newAction.Position + insertLength;
            return new DocumentAction(revision, newPosition, null, deleteLength, client);
        }

        var positionDelta = oldAction.Position - newAction.Position;
        if (positionDelta >= deleteLength)
            return new DocumentAction(revision, newAction.Position, null, deleteLength, client);
        
        var newDelete = deleteLength + insertLength;
        return new DocumentAction(revision, newAction.Position, oldAction.Insert, newDelete, client);
    }
    
    public static DocumentAction UpdateOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var deleteLength = newAction.Delete ?? 0;
        var newInsertLength = newAction.Insert?.Length ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        
        if (newAction.Position >= oldAction.Position)
        {
            var newPosition = newAction.Position + oldInsertLength;
            return new DocumentAction(revision, newPosition, newAction.Insert, deleteLength, client);
        }
        
        var positionDelta = oldAction.Position - newAction.Position;
        if (positionDelta >= deleteLength)
            return new DocumentAction(revision, newAction.Position, newAction.Insert, deleteLength, client);

        var newDelete = deleteLength + newInsertLength;
        var newInsert = (newAction.Insert ?? "") + (oldAction.Insert ?? "");
        return new DocumentAction(revision, newAction.Position, newInsert, newDelete, client);
    }
    
    public static DocumentAction InsertOverDelete(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var deleteLength = oldAction.Delete ?? 0;
        if (newAction.Position < oldAction.Position) return new DocumentAction(revision, newAction.Position, newAction.Insert, null, client);

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

        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
            {
                return new DocumentAction(revision, newAction.Position, null, newDelete, client);
            }

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            return new DocumentAction(revision, newAction.Position, null, positionDelta + deleteRemainingOrZero, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) return new DocumentAction(revision, newAction.Position - oldDelete, null, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
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
        
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
                return new DocumentAction(revision, newAction.Position, newAction.Insert, newAction.Delete, client);

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            
            return new DocumentAction(revision, newAction.Position, newAction.Insert, positionDelta + deleteRemainingOrZero, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) 
            return new DocumentAction(revision, newAction.Position - oldDelete, newAction.Insert, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        return deleteSurplus <= 0
            ? new DocumentAction(revision, newAction.Position - positionDif, newAction.Insert, 0, client)
            : new DocumentAction(revision, newAction.Position - positionDif, newAction.Insert, deleteSurplus, client);
    }

    public static DocumentAction InsertOverUpdate(DocumentAction newAction, DocumentAction oldAction)
    {   
        var revision = oldAction.Revision + 1;
        var client = newAction.Client;
        var oldDelete = oldAction.Delete ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var oldNetLength = oldInsertLength - oldDelete;
        
        if (newAction.Position < oldAction.Position)
            return new DocumentAction(revision, newAction.Position, newAction.Insert, null, client);

        var positionDelta = newAction.Position - oldAction.Position;
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
        
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
                return new DocumentAction(revision, newAction.Position, null, newAction.Delete, client);

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            return new DocumentAction(revision, newAction.Position, oldAction.Insert, positionDelta + deleteRemainingOrZero + oldInsertLength, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) 
            return new DocumentAction(revision, newAction.Position + oldNetLength, null, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
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
        
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
                return new DocumentAction(revision, newAction.Position, newAction.Insert, newAction.Delete, client);

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            return new DocumentAction(revision, newAction.Position, newAction.Insert + (oldAction.Insert ?? ""), positionDelta + oldInsertLength + deleteRemainingOrZero, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) 
            return new DocumentAction(revision, newAction.Position + oldNetLength, newAction.Insert, newDelete, client);
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        var deleteSurplusOrZero = deleteSurplus < 0 ? 0 : deleteSurplus;
        return new DocumentAction(revision, oldAction.Position + oldInsertLength, newAction.Insert, deleteSurplusOrZero, client);
    }
}