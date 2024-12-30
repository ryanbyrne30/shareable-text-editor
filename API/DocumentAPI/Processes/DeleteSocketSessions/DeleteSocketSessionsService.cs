using DocumentAPI.Repositories;

namespace DocumentAPI.Processes.DeleteSocketSessions;

public class DeleteSocketSessionsService(Repository repository, ILogger<DeleteSocketSessionsService> logger)
{
    public async Task DeleteSessions(string socketId)
    {
        logger.LogInformation("Delete sessions for socket {socketId}", socketId);
        var sessions = repository.Sessions.Where(s => s.SocketId == socketId);
        repository.Sessions.RemoveRange(sessions);
        await repository.SaveChangesAsync();
    }
    
}