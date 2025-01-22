using BackendService.Gateway.Utils;
using BackendService.Services.Documents.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.CreateDocument;

[ApiController]
public class CreateDocumentController(CreateDocumentService documentService): ControllerBase
{
    public const string Endpoint = "/api/v1/documents";
    
    [HttpPost(Endpoint)]
    [Authorize]
    public async Task<ActionResult<CreateDocumentResponse>> CreateDocument()
    {
        var userId = Authorize.GetRequiredCurrentUserId(HttpContext);
        var request = new CreateDocumentService.Request(userId, "Untitled");
        var documentId = await documentService.CreateDocument(request);
        return new CreateDocumentResponse
        {
            Id = documentId
        };
    }
}