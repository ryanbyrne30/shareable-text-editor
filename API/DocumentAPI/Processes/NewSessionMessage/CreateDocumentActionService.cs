using DocumentAPI.Domain;
using DocumentAPI.Repositories;

namespace DocumentAPI.Processes.NewSessionMessage;

public class CreateDocumentActionService(Repository repository)
{
    public async Task<string> CreateAction(string sessionId, ulong revision, ulong position, ulong deleted, string inserted)
    {
        var session = await repository.Sessions.FindAsync(sessionId);
        if (session == null)
        {
            throw new BadHttpRequestException("Session not found");
        }

        var id = Repository.GenerateId(DocumentAction.IdPrefix);
        var action = new DocumentAction
        {
            Id = id, 
            SessionId = session.Id,
            Revision = revision,
            Inserted = inserted,
            Deleted = deleted,
            Position = position,
            IsCompleted = false,
            OccurredAt = DateTime.Now
        };
        
        await repository.DocumentActions.AddAsync(action);
        await repository.SaveChangesAsync();
        return id;
    }
}