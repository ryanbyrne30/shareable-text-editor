using WebSocketAPI.Config;
using WebSocketAPI.Services.DocumentService.CreateSession;
using WebSocketAPI.Services.DocumentService.DeleteSession;
using WebSocketAPI.Services.DocumentService.SendMessage;

namespace WebSocketAPI.Services.DocumentService;

public class DocumentService(AppConfig config, HttpRequestService.HttpRequestService requestService, ILogger<DocumentService> logger)
{
    private readonly string _documentApiUrl = config.DocumentApiUrl;
    
    private string CreateUrl(string endpoint) => _documentApiUrl + endpoint;
    
    public async Task<CreateSessionResponse> CreateSession(CreateSessionRequest request)
    {
        logger.LogDebug("Creating session for document {docId}, socket {SocketId}", request.DocumentId, request.SocketId);
        return await requestService.Post<CreateSessionRequest, CreateSessionResponse>(CreateUrl("/sessions"), request);
    }
    
    public async Task DeleteSession(string sessionId)
    {
        logger.LogDebug("Deleting sessions for session {sessionId}", sessionId);
        await requestService.Delete<DeleteSessionResponse>(CreateUrl($"/sessions/session/{sessionId}"));
    }
    
    public async Task SendMessage(string sessionId, SendMessageRequest request)
    {
        logger.LogDebug("Sending message to document server for session {sessionId}: {message}", sessionId, request.Message);
        await requestService.Post<SendMessageRequest, SendMessageResponse>(CreateUrl($"/sessions/session/{sessionId}"), request);
    }
}