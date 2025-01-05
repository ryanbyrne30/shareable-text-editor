namespace DocumentAPI.Config;

public class AppConfig
{
    public required string ConnectionString { get; init; }
    public required string WebSocketApiUrl { get; init; }
}