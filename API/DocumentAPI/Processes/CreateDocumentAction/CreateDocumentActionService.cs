using DocumentAPI.Domain;
using DocumentAPI.Repositories;

namespace DocumentAPI.Processes.CreateDocumentAction;

public class CreateDocumentActionService(Repository repository)
{
    public async Task<string> CreateAction(string docId, CreateDocumentActionRequest request)
    {
        var document = await repository.Documents.FindAsync(docId);
        if (document == null)
        {
            throw new BadHttpRequestException("Document not found");
        }

        var session = repository.Sessions.FirstOrDefault(s => s.DocumentId == docId && s.SocketId == request.SocketId);
        if (session == null)
        {
            throw new BadHttpRequestException("Session not found");
        }

        var id = Repository.GenerateId(DocumentAction.IdPrefix);
        var action = new DocumentAction
        {
            Id = id, 
            SessionId = session.Id,
            Revision = request.Revision,
            Inserted = request.Inserted,
            Deleted = request.Deleted,
            Position = request.Position,
            IsCompleted = false,
            OccurredAt = DateTime.Now
        };
        
        await repository.DocumentActions.AddAsync(action);
        await repository.SaveChangesAsync();
        return id;
    }
}