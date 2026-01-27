using Application.Abstractions;
using Infrastructure.ExternalApi.Configurator;
using Infrastructure.ExternalApi.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.ExternalApi.Gites;

public sealed class HotelroomClient : IHotelroomReadClient
{
    private readonly HttpClient _http;
    private readonly HotelroomApiOptions _opt;

    public HotelroomClient(HttpClient http, IOptions<HotelroomApiOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
    }

    public async Task<HotelroomSnapshot> GetInfoAsync(HotelroomRequest request, CancellationToken ct)
    {
        var dto = await _http.GetJsonOrThrowAsync<HotelroomApiResponse>(
            apiName: "HotelroomApi",
            relativeUrl: $"hotelroom/{request.RoomId}",
            resourceType: "Hotelroom",
            resourceId: request.RoomId,
            ct: ct);

        return new HotelroomSnapshot(dto.RoomId, dto.HotelroomPrice, dto.IsAvailable, dto.CapacityMin, dto.CapacityMax);
    }

    private sealed class HotelroomApiResponse
    {
        [JsonPropertyName("RoomId")]
        public int RoomId { get; init; }

        [JsonPropertyName("HotelRoomPrice")]
        public decimal HotelroomPrice { get; init; }

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; init; }

        [JsonPropertyName("capacityMin")]
        public int CapacityMin { get; init; }

        [JsonPropertyName("capacityMax")]
        public int CapacityMax { get; init; }
    }
}
