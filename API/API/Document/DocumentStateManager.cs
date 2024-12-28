using API.Document.Domain;

namespace API.Document;

public static class DocumentStateManager
{
    public static async Task Run(string docId)
    {
        if (!DocumentService.DocumentExists(docId)) return;

        while(true) 
        {
            try
            {
                var pending = DocumentService.GetNextPendingDocumentAction(docId);
                if (pending == null) continue;
                var transformed = ApplyAction(docId, pending);
                var text = DocumentService.GetDocumentContent(docId);
                await DocumentClientService.BroadcastAction(docId, transformed);
            } catch (Exception e)
            {
                Console.WriteLine($"Error processing document {docId}: {e.Message}");
            }
        }
    }
    
    private static DocumentAction ApplyAction(string docId, DocumentAction action)
    {
        var completedActions = DocumentService.GetCompletedDocumentActions(docId);
        var newAction = OperationalTransformation.Transform(action, completedActions);
        DocumentService.ApplyAction(docId, newAction, action);
        return newAction;
    }
}