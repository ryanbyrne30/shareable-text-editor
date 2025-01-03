using DocumentAPI.Domain;

namespace DocumentAPI.Processes.DocumentWatcher;

public static class OperationalTransformation
{
    public static DocumentAction Transform(DocumentAction action, List<DocumentAction> completedActions)
    {
        var docVersion = completedActions.Count;
        if (action.Revision == docVersion + 1) return action;
        if (action.Revision > docVersion + 1) throw new Exception("Invalid revision number");

        var conflictingActions = completedActions.Slice( action.Revision - 1, completedActions.Count - action.Revision + 1);
        var transformedAction = conflictingActions.Aggregate(action, TransformAction);
        if (transformedAction == null) throw new Exception("Invalid transformation");
        return transformedAction;
    }

    public static DocumentAction TransformAction(DocumentAction newAction, DocumentAction oldAction)
    {
        var revision = oldAction.Revision + 1;
        
        // new update is before old update
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            // new update is way before old update => no change
            if (positionDelta > newAction.Deleted)
                return new DocumentAction
                {
                    Id = newAction.Id,
                    Revision = revision,
                    Position = newAction.Position,
                    Inserted = newAction.Inserted,
                    Deleted = newAction.Deleted,
                    DocumentId = newAction.DocumentId,
                    SessionId = newAction.SessionId
                };

            var deleteRemaining = Math.Max(0, newAction.Deleted - positionDelta - oldAction.Deleted);
            // new update is just before old update (conflict) => update, deleting excess characters
            return new DocumentAction
            {
                Id = newAction.Id,
                Revision = revision,
                Position = newAction.Position,
                Inserted = newAction.Inserted + oldAction.Inserted,
                Deleted = positionDelta + oldAction.Inserted.Length + deleteRemaining,
                DocumentId = newAction.DocumentId,
                SessionId = newAction.SessionId
            };
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        // new update is way after old update => offset new update by old net length
        if (positionDif > oldAction.Deleted)
            return new DocumentAction
            {
                Id = newAction.Id,
                Revision = revision,
                Position = newAction.Position + oldAction.Inserted.Length,
                Inserted = newAction.Inserted,
                Deleted = newAction.Deleted,
                DocumentId = newAction.DocumentId,
                SessionId = newAction.SessionId
            };
        
        var deleteSurplus = Math.Max(0, newAction.Deleted - oldAction.Deleted + positionDif);
        // new update is just after old update (conflict) => update from old position, deleting excess characters
        return new DocumentAction
        {
            Id = newAction.Id,
            Revision = revision,
            Position = oldAction.Position + oldAction.Inserted.Length,
            Inserted = newAction.Inserted,
            Deleted = deleteSurplus, 
            DocumentId = newAction.DocumentId,
            SessionId = newAction.SessionId
        };
    }
}