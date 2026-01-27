using Application.Abstractions;
using Infrastructure.ExternalApi.Configurator;
using Infrastructure.ExternalApi.Helpers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.ExternalApi.Gites;

public sealed class GiteClient : IGiteReadClient
{
    private readonly HttpClient _http;
    private readonly GiteApiOptions _opt;

    public GiteClient(HttpClient http, IOptions<GiteApiOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
    }

    public async Task<GiteSnapshot> GetInfoAsync(GiteRequest request, CancellationToken ct)
    {
        var dto = await _http.GetJsonOrThrowAsync<GiteApiResponse>(
            apiName: "GiteApi",
            relativeUrl: $"gite/get/{request.GiteId}",
            resourceType: "Gite",
            resourceId: request.GiteId,
            ct: ct);

        return new GiteSnapshot(dto.GiteId, dto.GitePrice, dto.IsAvailable, dto.CapacityMin, dto.CapacityMax);
    }


    // Vendor response shape stays private in Infrastructure
    private sealed class GiteApiResponse
    {
        [JsonPropertyName("giteId")]
        public int GiteId { get; init; }

        [JsonPropertyName("gitePrice")]
        public decimal GitePrice { get; init; }

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; init; }

        [JsonPropertyName("capacityMin")]
        public int CapacityMin { get; init; }

        [JsonPropertyName("capacityMax")]
        public int CapacityMax { get; init; }

        // Present in JSON but ignored unless you add properties:
        // giteNumber, giteAddress, amenities, beds...
    }
}
