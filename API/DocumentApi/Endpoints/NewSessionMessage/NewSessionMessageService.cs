namespace DocumentAPI.Endpoints.NewSessionMessage;

public class NewSessionMessageService(CreateDocumentActionService createDocumentActionService, ILogger<NewSessionMessageService> logger)
{
    public async Task HandleNewMessage(string sessionId, NewSessionMessageRequest request)
    {
        await createDocumentActionService.CreateAction(
            sessionId, 
            request.Action.Revision, 
            request.Action.Position, 
            request.Action.Deleted, 
            request.Action.Inserted);
    }
}