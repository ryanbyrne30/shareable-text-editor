using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Processes.CreateDocumentAction;

public class CreateDocumentActionController(CreateDocumentActionService service): ControllerBase
{
    [HttpPost("/docs/doc/{docId}/action")]
    public async Task<IActionResult> CreateAction(string docId, [FromBody] CreateDocumentActionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var id = await service.CreateAction(docId, request);
        var response = new CreateDocumentActionResponse { Id = id };
        return Ok(response);
    }
}