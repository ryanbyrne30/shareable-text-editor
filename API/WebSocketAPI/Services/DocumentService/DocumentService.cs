using WebSocketAPI.Config;
using WebSocketAPI.Services.DocumentService.CreateSession;
using WebSocketAPI.Services.DocumentService.DeleteSocketSessions;

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
    
    public async Task DeleteSocketSessions(string socketId)
    {
        logger.LogDebug("Deleting sessions for socket {socketId}", socketId);
        await requestService.Delete<DeleteSocketSessionsResponse>(CreateUrl($"/sockets/socket/{socketId}"));
    }
}