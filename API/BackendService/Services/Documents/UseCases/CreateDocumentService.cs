using BackendService.Common;
using BackendService.Common.Repositories;
using BackendService.Services.Documents.Domain;

namespace BackendService.Services.Documents.UseCases;

public class CreateDocumentService(AppRepository repository)
{
    public sealed record Request(string UserId, string Name);
    
    public async Task<string> CreateDocument(Request request)
    {
        var id = DatabaseUtil.GenerateId(Document.IdPrefix);
        var document = new Document
        {
            Id = id,
            Name = request.Name,
            UserId = request.UserId
        };

        await repository.Documents.AddAsync(document);
        return id;
    }
}