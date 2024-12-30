using DocumentAPI.Processes.DocumentWatcher;
using DocumentAPI.Repositories;

namespace DocumentAPI.Services.DocumentWatcherManager;

public class DocumentWatcherManager(ILoggerFactory loggerFactory, RepositoryFactory repositoryFactory, WebSocketAPIService.WebSocketApiService webSocketApiService)
{
    private static readonly Dictionary<string, DocumentWatcher> Watchers = new();

    public void WatchDocument(string docId)
    {
        if (Watchers.ContainsKey(docId)) return;
        
        var watcherLogger = loggerFactory.CreateLogger<DocumentWatcher>();
        var repository = repositoryFactory.CreateRepository();
        var watcher = new DocumentWatcher(watcherLogger, repository, webSocketApiService); 
        Watchers.Add(docId, watcher);
        
        Task.Run(() => watcher.Watch(docId)).ContinueWith(t =>
        {
            watcherLogger.LogDebug("DocumentWatcher for {docId} has stopped", docId);
            Watchers.Remove(docId);
        }, TaskScheduler.Current);
    }
}