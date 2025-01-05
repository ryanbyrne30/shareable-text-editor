using DocumentAPI.Domain;

namespace DocumentAPI.Processes.DocumentWatcher;

public static class OperationalTransformation
{
    public static void Transform(DocumentAction action, List<DocumentAction> completedActions)
    {
        var docVersion = completedActions.Count;
        if (action.Revision == docVersion + 1) return;
        if (action.Revision > docVersion + 1) throw new Exception("Invalid revision number");
        completedActions.ForEach(a => TransformAction(action, a));
    }

    private static void TransformAction(DocumentAction newAction, DocumentAction oldAction)
    {
        newAction.Revision = oldAction.Revision + 1;

        // new update is before old update
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            // new update is way before old update => no change
            if (positionDelta > newAction.Deleted) return;

            var deleteRemaining = Math.Max(0, newAction.Deleted - positionDelta - oldAction.Deleted);
            // new update is just before old update (conflict) => update, deleting excess characters
            newAction.Inserted = newAction.Inserted + oldAction.Inserted;
            newAction.Deleted = positionDelta + oldAction.Inserted.Length + deleteRemaining;
            return;
        }

        var positionDif = newAction.Position - oldAction.Position;
        // new update is way after old update => offset new update by old net length
        if (positionDif > oldAction.Deleted)
        {
            newAction.Position += oldAction.Inserted.Length;
            return;
        }

        var deleteSurplus = Math.Max(0, newAction.Deleted - oldAction.Deleted + positionDif);
        // new update is just after old update (conflict) => update from old position, deleting excess characters
        newAction.Position = oldAction.Position + oldAction.Inserted.Length;
        newAction.Deleted = deleteSurplus;
    }
}