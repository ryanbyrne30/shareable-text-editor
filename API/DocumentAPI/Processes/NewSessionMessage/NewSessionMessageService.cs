using System.Text.Json;

namespace DocumentAPI.Processes.NewSessionMessage;

public class NewSessionMessageService(CreateDocumentActionService createDocumentActionService)
{
    public async Task HandleNewMessage(string sessionId, NewSessionMessageRequest request)
    {
        var message = JsonSerializer.Deserialize<NewSessionMessage>(request.Message);
        if (message == null) 
            throw new BadHttpRequestException("Invalid message");
        if (message.NewAction != null)
            await createDocumentActionService.CreateAction(sessionId, message.NewAction.Revision, message.NewAction.Position, message.NewAction.Deleted, message.NewAction.Inserted);
    }
}