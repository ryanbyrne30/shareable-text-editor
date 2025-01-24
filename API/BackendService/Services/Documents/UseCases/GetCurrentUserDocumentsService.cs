using System.Linq.Dynamic.Core;
using BackendService.Common.Repositories;

namespace BackendService.Services.Documents.UseCases;

public class GetCurrentUserDocumentsService(AppRepository repository)
{
    public sealed record Request(string UserId, int Page = 1, int PageSize = 10, string SortBy = "UpdatedAt", string SortDirection = "desc");
    
    public sealed record Response(int Total, List<DocumentType> Documents);
    
    public Response GetCurrentUserDocuments(Request request)
    {
        var offset = (request.Page - 1) * request.PageSize;
        var documents = repository.Documents.Where(d => d.UserId == request.UserId)
            .Select(d => new DocumentType
            {
                Id = d.Id, 
                Name = d.Name,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            })
            .OrderBy(request.SortBy + " " + request.SortDirection)
            .Skip(offset)
            .Take(request.PageSize)
            .ToList();
        var documentsCount = repository.Documents.Count(d => d.UserId == request.UserId);
        return new Response(documentsCount, documents);
    }
    
    public class DocumentType
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required DateTime CreatedAt{ get; set; }
        public required DateTime? UpdatedAt { get; set; }
    }
}