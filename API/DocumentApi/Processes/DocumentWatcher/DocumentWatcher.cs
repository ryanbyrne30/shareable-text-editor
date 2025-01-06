using DocumentAPI.Domain;
using DocumentAPI.Repositories;
using DocumentAPI.Services.WebSocketAPIService;

namespace DocumentAPI.Processes.DocumentWatcher;

public class DocumentWatcher(ILogger<DocumentWatcher> logger, Repository repository, IWebSocketApiService webSocketService)
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

            var completedActions = QueryCompletedActions(docId, result.Action.Revision);
            var pendingAction = result.Action;

            try
            {
                OperationalTransformation.Transform(pendingAction, completedActions);
                pendingAction.CompletedAt = DateTime.Now;
                repository.DocumentActions.Update(pendingAction);
                await repository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to update pending action {pendingAction}", pendingAction);
                return;
            }

            if (result.Session != null) await SendAck(result.Session.SocketId, pendingAction);

            var sessions = repository.Sessions.Where(s => s.DocumentId == docId).ToList();
            foreach (var session in sessions)
            {
                if (session.Id == result.Session?.Id) continue;
                await SendAction(session.SocketId, pendingAction);
            }

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
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to query pending action for document {docId}", docId);
            return null;
        }
    }

    private List<DocumentAction> QueryCompletedActions(string docId, int revision)
    {
        try
        {
            return repository.DocumentActions.Where(a => a.DocumentId == docId && a.CompletedAt != null && a.Revision >= revision).OrderBy(a => a.Revision).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to query completed actions for document {docId}", docId);
            return [];
        }
    }

    private async Task SendAck(string socketId, DocumentAction action)
    {
        try
        {
            var message = SocketMessage.CreateAck(action.Revision);
            await webSocketService.SendMessageToSocket(socketId, message);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to send ACK to socket {socketId}. May have disconnected?", socketId);
        }
    }

    private async Task SendAction(string socketId, DocumentAction action)
    {
        try
        {
            var message = SocketMessage.CreateAction(action.Revision, action.Position, action.Inserted, action.Deleted);
            await webSocketService.SendMessageToSocket(socketId, message);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to send action to socket {socketId}. May have disconnected?", socketId);
        }
    }
}