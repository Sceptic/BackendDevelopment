using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FullSystemIntegrationTesting;

internal static class Http
{
    public static HttpClient CreateClient(Uri baseUrl)
    {
        var client = new HttpClient { BaseAddress = baseUrl };

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    public static async Task<JsonDocument> ReadJsonAsync(HttpResponseMessage response)
    {
        var stream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(stream);
    }

    public static async Task<HttpResponseMessage> PostJsonStringAsync(this HttpClient client, string path, string json)
    {
        return await client.PostAsync(path, new StringContent(json, Encoding.UTF8, "application/json"));
    }

    public static async Task<HttpResponseMessage> PutJsonStringAsync(this HttpClient client, string path, string json)
    {
        return await client.PutAsync(path, new StringContent(json, Encoding.UTF8, "application/json"));
    }

    public static async Task<HttpResponseMessage> PatchJsonStringAsync(this HttpClient client, string path, string json)
    {
        var req = new HttpRequestMessage(new HttpMethod("PATCH"), path)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        return await client.SendAsync(req);
    }

    public static int ExtractTrailingIntIdFromLocation(HttpResponseMessage response)
    {
        if (response.Headers.Location is null)
            throw new InvalidOperationException("Expected Location header to be present, but it was missing.");

        var location = response.Headers.Location.ToString().TrimEnd('/');
        var last = location.Split('/').Last();

        if (!int.TryParse(last, out var id) || id <= 0)
            throw new InvalidOperationException($"Could not parse id from Location header: '{location}'");

        return id;
    }

    public static void AssertStatus(HttpStatusCode expected, HttpResponseMessage response)
    {
        if (response.StatusCode != expected)
        {
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new Xunit.Sdk.XunitException($"Expected {(int)expected} {expected} but got {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
        }
    }
}
