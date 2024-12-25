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
        return newDocument;
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
}