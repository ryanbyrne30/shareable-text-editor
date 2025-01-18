namespace BackendService.Common;

public static class DatabaseUtil
{
    public static string GenerateId(string prefix)
    {
        var id = Guid.NewGuid().ToString().Replace("-", "");
        return $"{prefix}_{id}";
    } 
    
}