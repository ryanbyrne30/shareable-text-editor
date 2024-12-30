using DocumentAPI.Domain;
using DocumentAPI.Repositories;
using DocumentAPI.Services.WebSocketAPIService;

namespace DocumentAPI.Processes.DocumentWatcher;

public class DocumentWatcher(ILogger<DocumentWatcher> logger, Repository repository, WebSocketApiService webSocketService)
{
    public async Task Watch(string docId)
    {
        while (true)
        {
            var result = QueryPendingAction(docId);
            if (result == null)
            {
                logger.LogDebug("No pending actions for document {docId}", docId);
                return;
            }
            
            var pendingAction = result.Action;

            try
            {
                pendingAction.CompletedAt = DateTime.Now;
                repository.DocumentActions.Update(pendingAction);
                await repository.SaveChangesAsync();
            } catch (Exception e)
            {
                logger.LogError(e, "Failed to update pending action {pendingAction}", pendingAction);
                return;
            }
            
            if (result.Session != null) await SendAck(result.Session.SocketId, pendingAction); 
            
            logger.LogDebug("Pending action: {pendingAction} resolved", pendingAction);
        }
    }

    private QueryPendingActionResult? QueryPendingAction(string docId)
    {
        try
        {
            var query = from a in repository.DocumentActions
                where a.DocumentId == docId && a.CompletedAt == null
                join s in repository.Sessions
                    on a.SessionId equals s.Id
                orderby a.Revision
                select new QueryPendingActionResult
                {
                    Action = a,
                    Session = s
                };
            
            return query.FirstOrDefault();
        } catch (Exception e)
        {
            logger.LogError(e, "Failed to query pending action for document {docId}", docId);
            return null;
        }
    }
    
    private async Task SendAck(string socketId, DocumentAction action)
    {
        try
        {
            await webSocketService.SendMessageToSocket(socketId, $"ACK: {action.Revision}");
        } catch (Exception e)
        {
            logger.LogWarning(e, "Failed to send ACK to socket {socketId}. May have disconnected?", socketId);
        }
    }
}