using DocumentAPI.Repositories;

namespace DocumentAPI.Processes.DeleteSession;

public class DeleteSessionService(Repository repository, ILogger<DeleteSessionService> logger)
{
    public async Task DeleteSessions(string socketId)
    {
        logger.LogInformation("Delete sessions for socket {socketId}", socketId);
        var sessions = repository.Sessions.Where(s => s.SocketId == socketId);
        repository.Sessions.RemoveRange(sessions);
        await repository.SaveChangesAsync();
    }
    
}