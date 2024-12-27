using API.Document.Domain;

namespace API.Document;

public static class DocumentService
{
    private static readonly Dictionary<string, Domain.Document> Documents = [];
    
    private static Domain.Document GetOrCreateDocument(string documentId)
    {
        if (Documents.TryGetValue(documentId, out var document))
        {
            return document;
        }

        var newDocument = new Domain.Document
        {
            Id = documentId,
        };
        Documents.Add(documentId, newDocument);
        Task.Run(() => DocumentStateManager.Run(newDocument.Id));
        return newDocument;
    }

    public static void Clear()
    {
        Documents.Clear();
    }
    
    public static void NewClient(string documentId, DocumentClient client)
    {
        var document = GetOrCreateDocument(documentId);
        document.Clients.Add(client);
    }
    
    public static void RemoveClient(string documentId, DocumentClient client)
    {
        if (Documents.TryGetValue(documentId, out var document))
        {
            document.Clients.RemoveAll(c => c.Id == client.Id);
        }
    }
    
    public static int GetDocumentVersion(string documentId)
    {
        if (!Documents.TryGetValue(documentId, out var document))
        {
            return -1;
        }
        return document.CompletedActions.Count;
    }
    
    public static void NewAction(string documentId, DocumentAction action)
    {
        var document = GetOrCreateDocument(documentId);
        document.PendingActions.Add(action);
    }

    public static bool DocumentExists(string docId)
    {
        return Documents.ContainsKey(docId);
    }
    
    public static DocumentAction? GetNextPendingDocumentAction(string documentId)
    {
        return Documents.TryGetValue(documentId, out var document) ? document.PendingActions.FirstOrDefault() : null;
    }
    
    public static List<DocumentAction> GetCompletedDocumentActions(string documentId)
    {
        return Documents.TryGetValue(documentId, out var document) ? document.CompletedActions : [];
    }
    
    public static void ResolvePendingDocumentAction(string documentId, DocumentAction action)
    {
        if (!Documents.TryGetValue(documentId, out var document)) return;
        document.CompletedActions.Add(action);
        document.PendingActions.Remove(action);
    }
    
    public static void ApplyAction(string docId, DocumentAction action)
    {
        var document = GetOrCreateDocument(docId);
        var content = document.Content;
        document.Content = action.Apply(content);
    }

    public static List<DocumentClient> GetDocumentClients(string docId)
    {
        return Documents.TryGetValue(docId, out var document) ? document.Clients : [];
    }
}