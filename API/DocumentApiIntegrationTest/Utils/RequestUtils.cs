using System.Text;
using System.Text.Json;

namespace DocumentAPIIntegrationTests.Utils;

public class RequestUtils
{
    public static StringContent JsonContent(object obj) =>
        new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
}