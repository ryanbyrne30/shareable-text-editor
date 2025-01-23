using BackendService.Common.Repositories;
using BackendService.Services.Documents.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Services.Documents.UseCases;

public class GetDocumentByIdService(AppRepository repository)
{
    public async Task<Document?> GetDocumentById(string id)
    {
        return await repository.Documents.FirstOrDefaultAsync(d => d.Id == id);
    }
    
}