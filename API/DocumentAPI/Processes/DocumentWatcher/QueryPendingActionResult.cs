using DocumentAPI.Domain;

namespace DocumentAPI.Processes.DocumentWatcher;

public class QueryPendingActionResult
{
    public required DocumentAction Action { get; set; }
    public required Session? Session { get; set; }
}