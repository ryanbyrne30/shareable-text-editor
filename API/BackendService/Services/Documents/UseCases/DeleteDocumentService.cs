using System.Net;
using BackendService.Common.Exceptions;
using BackendService.Common.Repositories;

namespace BackendService.Services.Documents.UseCases;

public class DeleteDocumentService(AppRepository repository)
{
    public async Task DeleteDocument(string userId, string documentId)
    {
        var document = await repository.Documents.FindAsync(documentId);
        if (document == null) throw new BadRequestException("Document not found", HttpStatusCode.NotFound);
        if (document.UserId != userId) throw new UnauthorizedRequestException();
        repository.Documents.Remove(document);
        await repository.SaveChangesAsync();
    }
    
}