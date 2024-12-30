using System.Text.Json;

namespace DocumentAPI.Endpoints.NewSessionMessage;

public class NewSessionMessageService(CreateDocumentActionService createDocumentActionService, ILogger<NewSessionMessageService> logger)
{
    public async Task HandleNewMessage(string sessionId, NewSessionMessageRequest request)
    {
        var message = ParseMessage(request.Message);
        if (message == null) 
            throw new BadHttpRequestException("Invalid message");
        if (message.NewAction != null)
            await createDocumentActionService.CreateAction(sessionId, message.NewAction.Revision, message.NewAction.Position, message.NewAction.Deleted, message.NewAction.Inserted);
    }
    
    private NewSessionMessage? ParseMessage(string message)
    {
        try
        {
            return JsonSerializer.Deserialize<NewSessionMessage>(message);
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Failed to parse message");
            return null;
        }
    }
}