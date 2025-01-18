using System.Text.Json;

namespace BackendService_IntegrationTests.Utils;

public static class RequestUtils
{
    public static T ParseResponse<T>(HttpResponseMessage response)
    {
        var content = response.Content.ReadAsStringAsync().Result;
        var parsed = JsonSerializer.Deserialize<T>(content);
        if (parsed == null)
            throw new Exception($"Failed to parse response: {content}");
        return parsed;
    }
    
}