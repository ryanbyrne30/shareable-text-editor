using BackendService.Gateway.Utils;
using BackendService.Services.Documents.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.DeleteDocument;

[ApiController]
public class DeleteDocumentController(DeleteDocumentService deleteDocumentService): ControllerBase
{
    public const string Endpoint = "/api/v1/documents/{id}";
    
    [HttpDelete(Endpoint)]
    [Authorize]
    public async Task<ActionResult<DeleteDocumentResponse>> DeleteDocument(string id)
    {
        var userId = Authorize.GetRequiredCurrentUserId(HttpContext);
        await deleteDocumentService.DeleteDocument(userId, id);
        return new DeleteDocumentResponse {Message = "Document deleted"};
    }
}