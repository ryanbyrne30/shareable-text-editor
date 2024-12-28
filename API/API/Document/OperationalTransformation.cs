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

    public static DocumentAction TransformAction(DocumentAction newAction, DocumentAction oldAction)
    {
        newAction.Insert ??= "";
        newAction.Delete ??= 0;
        oldAction.Insert ??= "";
        oldAction.Delete ??= 0;
        return UpdateOverUpdate(newAction, oldAction);
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

            var deleteRemaining = Math.Max(0, newDelete - positionDelta - oldDelete);
            // new update is just before old update (conflict) => update, deleting excess characters
            return new DocumentAction(revision, newAction.Position, newAction.Insert + (oldAction.Insert ?? ""), positionDelta + oldInsertLength + deleteRemaining, client);
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        // new update is way after old update => offset new update by old net length
        if (positionDif > oldDelete) 
            return new DocumentAction(revision, newAction.Position + oldNetLength, newAction.Insert, newDelete, client);
        
        var deleteSurplus = Math.Max(0, newDelete - oldDelete + positionDif);
        // new update is just after old update (conflict) => update from old position, deleting excess characters
        return new DocumentAction(revision, oldAction.Position + oldInsertLength, newAction.Insert, deleteSurplus, client);
    }
}