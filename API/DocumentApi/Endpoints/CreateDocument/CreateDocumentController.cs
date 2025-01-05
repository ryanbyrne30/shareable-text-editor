using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Endpoints.CreateDocument;

public class CreateDocumentController(CreateDocumentService service) : ControllerBase
{
    [HttpPost("/docs")]
    public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var docId = await service.CreateDocument(request);
        var response = new CreateDocumentResponse { Id = docId };
        return Ok(response);
    }
}