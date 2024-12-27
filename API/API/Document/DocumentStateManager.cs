using API.Document.Domain;

namespace API.Document;

public static class DocumentStateManager
{
    public static async Task Run(string docId)
    {
        if (!DocumentService.DocumentExists(docId)) return;

        while(true) 
        {
            var pending = DocumentService.GetNextPendingDocumentAction(docId);
            if (pending == null) continue;
            var transformed = ApplyAction(docId, pending);
            await DocumentClientService.BroadcastAction(docId, transformed);
        }
    }
    
    private static DocumentAction ApplyAction(string docId, DocumentAction action)
    {
        var completedActions = DocumentService.GetCompletedDocumentActions(docId);
        var newAction = OperationalTransformation.Transform(action, completedActions);
        DocumentService.ApplyAction(docId, newAction);
        return newAction;
    }
}