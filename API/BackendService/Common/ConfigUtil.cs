namespace BackendService.Common;

public static class ConfigUtil
{
    public static string GetEnvVar(IConfiguration configuration, string name)
    {
        return configuration[name] ?? throw new Exception($"{name} not found.");
    }
}