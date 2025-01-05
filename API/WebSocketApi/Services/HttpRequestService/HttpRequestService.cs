using System.Text;
using System.Text.Json;

namespace WebSocketAPI.Services.HttpRequestService;

public class HttpRequestService(ILogger<HttpRequestService> logger)
{
    private static readonly HttpClient Client = new();
    
    public async Task<TResponse> Get<TResponse>(string url)
    {
        return await SendRequest<string, TResponse>(HttpMethod.Get, url, "", false);
    }
    
    public async Task<TResponse> Post<TRequest, TResponse>(string url, TRequest content)
    {
        return await SendRequest<TRequest, TResponse>(HttpMethod.Post, url, content);
    }
    
    public async Task<TResponse> Put<TRequest, TResponse>(string url, TRequest content)
    {
        return await SendRequest<TRequest, TResponse>(HttpMethod.Put, url, content);
    }
    
    public async Task<TResponse> Delete<TResponse>(string url)
    {
        return await SendRequest<string, TResponse>(HttpMethod.Delete, url, "", false);
    }
    
    public async Task<TResponse> Patch<TRequest, TResponse>(string url, TRequest content)
    {
        return await SendRequest<TRequest, TResponse>(HttpMethod.Patch, url, content);
    }
    
    private async Task<TResponse> SendRequest<TRequest, TResponse>(HttpMethod method, string url, TRequest content, bool hasBody = true)
    {
        logger.LogDebug("Sending {Method} request to {Url}", method, url);
        var request = new HttpRequestMessage(method, url);
        if (content != null && hasBody)
        {
            var body = JsonSerializer.Serialize(content);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        }
        
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        try
        {
            var result = JsonSerializer.Deserialize<TResponse>(responseBody);
            
            if (result == null) throw new JsonException("Could not parse JSON response");
            return result;
        } catch (JsonException e)
        {
            logger.LogError(e, "Failed to parse JSON response from {Url}", url);
            throw new HttpRequestException("Unexpected response from server", e);
        }
    }
}