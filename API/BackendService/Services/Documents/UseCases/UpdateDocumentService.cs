using System.Net;
using BackendService.Common.Exceptions;
using BackendService.Common.Repositories;

namespace BackendService.Services.Documents.UseCases;

public class UpdateDocumentService(AppRepository repository)
{
    public sealed record Request(string Id, string? Name, string? Content);
    
    public async Task UpdateDocument(Request request)
    {
        var document = await repository.Documents.FindAsync(request.Id);
        if (document is null) throw new BadRequestException("Document not found", HttpStatusCode.NotFound);
        if (request.Name is not null) document.Name = request.Name;
        if (request.Content is not null) document.Content = request.Content;
        document.UpdatedAt = DateTime.Now.ToUniversalTime();
        repository.Documents.Update(document);
        await repository.SaveChangesAsync();
    }
    
}