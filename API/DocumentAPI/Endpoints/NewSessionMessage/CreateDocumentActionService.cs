using DocumentAPI.Domain;
using DocumentAPI.Repositories;
using DocumentAPI.Services.DocumentWatcherManager;

namespace DocumentAPI.Endpoints.NewSessionMessage;

public class CreateDocumentActionService(Repository repository, DocumentWatcherManager documentWatcherManager)
{
    public async Task<string> CreateAction(string sessionId, long revision, ulong position, ulong deleted, string inserted)
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
            DocumentId = session.DocumentId,
            Revision = revision,
            Inserted = inserted,
            Deleted = deleted,
            Position = position,
            OccurredAt = DateTime.Now
        };
        
        await repository.DocumentActions.AddAsync(action);
        await repository.SaveChangesAsync();
        
        documentWatcherManager.WatchDocument(session.DocumentId);
        
        return id;
    }
}