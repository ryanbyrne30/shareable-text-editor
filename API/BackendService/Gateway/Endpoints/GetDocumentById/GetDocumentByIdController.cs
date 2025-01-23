using System.Net;
using BackendService.Common.Exceptions;
using BackendService.Services.Documents.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.GetDocumentById;

[ApiController]
public class GetDocumentByIdController(GetDocumentByIdService getDocumentByIdService): ControllerBase
{
    public const string Endpoint = "/api/v1/documents/{id}";
    
    [HttpGet(Endpoint)]
    public async Task<ActionResult<GetDocumentByIdResponse>> GetDocumentById(string id)
    {
        var document = await getDocumentByIdService.GetDocumentById(id);
        if (document == null) throw new BadRequestException("Document not found", HttpStatusCode.NotFound);
        return new GetDocumentByIdResponse
        {
            Id = document.Id,
            Name = document.Name
        };
    }
    
}