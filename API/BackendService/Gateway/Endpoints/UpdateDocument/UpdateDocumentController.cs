using System.ComponentModel.DataAnnotations;
using BackendService.Services.Documents.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.UpdateDocument;

[ApiController]
public class UpdateDocumentController(UpdateDocumentService updateDocumentService): ControllerBase
{
    public const string Endpoint = "/api/v1/documents/{id}";
    
    [HttpPatch(Endpoint)]
    public async Task<ActionResult<UpdateDocumentResponse>> UpdateDocument([FromRoute] [MaxLength(100)] string id, [FromBody] UpdateDocumentRequest request)
    {
        var updateRequest = new UpdateDocumentService.Request(id, Name: request.Name, request.Content);
        await updateDocumentService.UpdateDocument(updateRequest);
        return new UpdateDocumentResponse
        {
            Message = "Ok"
        };
    }
}