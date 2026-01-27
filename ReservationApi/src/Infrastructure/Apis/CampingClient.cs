using Application.Abstractions;
using Infrastructure.ExternalApi.Configurator;
using Infrastructure.ExternalApi.Helpers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.ExternalApi.Gites;

public sealed class CampingClient : ICampingReadClient
{
    private readonly HttpClient _http;
    private readonly CampingApiOptions _opt;

    public CampingClient(HttpClient http, IOptions<CampingApiOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
    }

    public async Task<CampingSnapshot> GetInfoAsync(CampingRequest request, CancellationToken ct)
    {
        var list = await _http.GetJsonOrThrowAsync<List<CampingApiResponse>>(
            apiName: "CampingApi",
            relativeUrl: $"api/Camping/{request.CampingId}/0/false?AccommodatieID=0&IncludeAccommodatie=false",
            resourceType: "Camping",
            resourceId: request.CampingId,
            ct: ct);

        var dto = list.FirstOrDefault()
                  ?? throw new ExternalApiException("CampingApi", $"Camping {request.CampingId} returned empty list.");

        return new CampingSnapshot(dto.CampingId);
    }

    private sealed class CampingApiResponse
    {
        [JsonPropertyName("campingID")]
        public int CampingId { get; init; }
    }
}
