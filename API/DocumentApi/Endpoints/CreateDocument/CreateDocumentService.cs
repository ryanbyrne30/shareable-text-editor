using DocumentAPI.Domain;
using DocumentAPI.Repositories;

namespace DocumentAPI.Endpoints.CreateDocument;

public class CreateDocumentService(Repository repository)
{
    public async Task<string> CreateDocument(CreateDocumentRequest request)
    {
        var docId = Repository.GenerateId(Document.IdPrefix);
        var document = new Document(docId, request.Title);
        await repository.Documents.AddAsync(document);
        await repository.SaveChangesAsync();
        return docId;
    }
}