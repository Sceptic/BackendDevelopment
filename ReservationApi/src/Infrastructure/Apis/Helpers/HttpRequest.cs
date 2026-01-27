using Azure.Core;
using Infrastructure.ExternalApi.Helpers;
using System.Net;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Infrastructure.ExternalApi;

//The given code handles the direct communication with the API at runtime with the requested parameters.
internal static class HttpClientExtensions
{
    internal static async Task<T> GetJsonOrThrowAsync<T>(
        this HttpClient http,
        string apiName,
        string relativeUrl,
        string resourceType,
        object resourceId,
        CancellationToken ct)
    {
        using var resp = await http.GetAsync(relativeUrl, ct);

        if (resp.StatusCode == HttpStatusCode.NotFound)
            throw new ResourceNotFoundException(apiName, resourceType, resourceId);

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new ExternalApiException(apiName, $"API failed ({(int)resp.StatusCode}): {body}");
        }

        return await resp.Content.ReadFromJsonAsync<T>(cancellationToken: ct)
               ?? throw new ExternalApiException(apiName, "Empty/invalid JSON.");
    }
}
