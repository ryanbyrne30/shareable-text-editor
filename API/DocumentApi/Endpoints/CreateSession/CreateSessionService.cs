using DocumentAPI.Domain;
using DocumentAPI.Repositories;

namespace DocumentAPI.Endpoints.CreateSession;

public class CreateSessionService(Repository repository, ILogger<CreateSessionService> logger)
{
    public async Task<string> CreateSession(CreateSessionRequest request)
    {
        logger.LogInformation("Creating session for document {DocumentId}, socket {SocketId}", request.DocumentId, request.SocketId);
        var document = await repository.Documents.FindAsync(request.DocumentId);
        if (document == null)
        {
            throw new BadHttpRequestException("Document not found");
        }

        var sessionId = Repository.GenerateId(Session.IdPrefix);
        var session = new Session
        {
            Id = sessionId,
            DocumentId = request.DocumentId,
            SocketId = request.SocketId,
            CreatedAt = DateTime.Now
        };
        await repository.Sessions.AddAsync(session);
        await repository.SaveChangesAsync();
        return sessionId;
    }
}