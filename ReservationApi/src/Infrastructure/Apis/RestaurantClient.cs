using Application.Abstractions;
using Infrastructure.ExternalApi.Configurator;
using Infrastructure.ExternalApi.Helpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.ExternalApi.Gites;

public sealed class RestaurantClient : IRestaurantReadClient
{
    private readonly HttpClient _http;
    private readonly RestaurantApiOptions _opt;

    public RestaurantClient(HttpClient http, IOptions<RestaurantApiOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
    }

    public async Task<RestaurantSnapshot> GetInfoAsync(RestaurantRequest request, CancellationToken ct)
    {
        var list = await _http.GetJsonOrThrowAsync<List<RestaurantApiResponse>>(
            apiName: "RestaurantApi",
            relativeUrl: $"api/Tafels?id={request.TableId}",
            resourceType: "Table",
            resourceId: request.TableId,
            ct: ct);

        var dto = list.FirstOrDefault();
        if (dto is null)
            throw new ResourceNotFoundException("RestaurantApi", "Table", request.TableId);

        return new RestaurantSnapshot(dto.TableId, dto.Capacity);
    }

    private sealed class RestaurantApiResponse
    {
        [JsonPropertyName("tafelID")]
        public int TableId { get; init; }

        [JsonPropertyName("aantalPlaatsen")]
        public int Capacity { get; init; }
    }
}
