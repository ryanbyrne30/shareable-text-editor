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
            ApplyAction(docId, pending);
            DocumentService.ResolvePendingDocumentAction(docId, pending);
            await DocumentClientService.BroadcastAction(docId, pending);
        }
    }
    
    private static void ApplyAction(string docId, DocumentAction action)
    {
        var version = DocumentService.GetDocumentVersion(docId);
        if (version < action.Revision) DocumentService.ApplyAction(docId, action);
        else
        {
            Console.WriteLine($"Unhandled condition for action. Current version: {version}. Action: {action.ToString()}");
        }
    }
}