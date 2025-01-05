using DocumentAPI.Repositories;

namespace DocumentAPI.Endpoints.DeleteSession;

public class DeleteSessionService(Repository repository, ILogger<DeleteSessionService> logger)
{
    public async Task DeleteSession(string sessionId)
    {
        logger.LogInformation("Delete session {sessionId}", sessionId);
        var session = await repository.Sessions.FindAsync(sessionId);
        if (session == null) throw new BadHttpRequestException("Session not found");
        repository.Sessions.Remove(session);
        await repository.SaveChangesAsync();
    }

}