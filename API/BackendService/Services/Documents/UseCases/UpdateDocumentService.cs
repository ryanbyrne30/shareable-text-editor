using System.Net;
using BackendService.Common.Exceptions;
using BackendService.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Services.Documents.UseCases;

public class UpdateDocumentService(AppRepository repository)
{
    public sealed record Request(string Id, string? Name);
    
    public async Task UpdateDocument(Request request)
    {
        var document = await repository.Documents.FindAsync(request.Id);
        if (document is null) throw new BadRequestException("Document not found", HttpStatusCode.NotFound);
        if (request.Name is not null) document.Name = request.Name;
        repository.Documents.Update(document);
        await repository.SaveChangesAsync();
    }
    
}