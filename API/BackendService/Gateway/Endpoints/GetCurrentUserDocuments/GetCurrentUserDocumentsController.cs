using BackendService.Gateway.Attributes;
using BackendService.Gateway.Utils;
using BackendService.Services.Documents.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.GetCurrentUserDocuments;

[ApiController]
[Authorize]
public class GetCurrentUserDocumentsController(GetCurrentUserDocumentsService getCurrentUserDocumentsService): ControllerBase
{
    [HttpGet("/api/v1/users/me/documents")]
    public ActionResult<GetCurrentUserDocumentsResponse> GetDocuments([FromQuery] GetCurrentUserDocumentsRequest request)
    {
        var userId = Authorize.GetRequiredCurrentUserId(HttpContext);
        var fetchRequest = new GetCurrentUserDocumentsService.Request(
            userId,
            request.Page,
            request.PageSize,
            DocumentSortByValidationAttribute.ValidValues[request.SortBy],
            request.SortDirection
        );
        var fetchResponse = getCurrentUserDocumentsService.GetCurrentUserDocuments(fetchRequest);
        
        var documents = fetchResponse.Documents.Select(d => new GetCurrentUserDocumentsResponse.DocumentType
        {
            Id = d.Id,
            Name = d.Name,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt
        }).ToList();
        
        return new GetCurrentUserDocumentsResponse
        {
            Documents = documents,
            Total = fetchResponse.Total
        };
    }
    
}