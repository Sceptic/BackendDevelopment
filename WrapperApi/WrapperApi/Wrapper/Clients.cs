using System.Net;
using System.Net.Http.Json;

namespace WrapperApi.Wrapper;

public sealed class AccountsApiClient
{
    private readonly HttpClient _http;
    public AccountsApiClient(HttpClient http) => _http = http;

    // LegacyMonolith: GET api/accounts/{accountId}
    public async Task<bool> Exists(int accountId, CancellationToken ct = default)
    {
        var resp = await _http.GetAsync($"api/accounts/{accountId}", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return false;
        resp.EnsureSuccessStatusCode();
        return true;
    }
}

public sealed class HotelApiClient
{
    private readonly HttpClient _http;
    public HotelApiClient(HttpClient http) => _http = http;

    // In jouw huidige implementatie heb je een get-all die een lijst teruggeeft.
    // LegacyMonolith had alleen GET api/hotelrooms/{roomNumber} (geen get-all).
    // Als jouw service geen get-all heeft: maak in die service een get-all erbij, of pas dit pad aan.
    public async Task<List<HotelRoomListItemDto>> GetAll(CancellationToken ct = default)
    {
        var rooms = await _http.GetFromJsonAsync<List<HotelRoomListItemDto>>("hotelroom", ct);
        return rooms ?? new List<HotelRoomListItemDto>();
    }

    public async Task<bool> ExistsRoom(int roomNumber, CancellationToken ct = default)
    {
        var resp = await _http.GetAsync($"hotelroom/{roomNumber}", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return false;
        resp.EnsureSuccessStatusCode();
        return true;
    }
}

public sealed class GiteApiClient
{
    private readonly HttpClient _http;
    public GiteApiClient(HttpClient http) => _http = http;

    public async Task<List<GiteListItemDto>> GetAll(CancellationToken ct = default)
    {
        var gites = await _http.GetFromJsonAsync<List<GiteListItemDto>>("gite/get/all", ct);
        return gites ?? new List<GiteListItemDto>();
    }

    public async Task<bool> ExistsGite(int giteNumber, CancellationToken ct = default)
    {
        var resp = await _http.GetAsync($"gite/get/{giteNumber}", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return false;
        resp.EnsureSuccessStatusCode();
        return true;
    }
}

public sealed class DalApiClient
{
    private readonly HttpClient _http;
    public DalApiClient(HttpClient http) => _http = http;

    // Assumptie: DAL exposeert GET api/reservations en GET api/reservations/{id}
    public async Task<ReservationDto?> GetReservation(int id, CancellationToken ct = default)
    {
        var resp = await _http.GetAsync($"api/reservations/{id}", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return null;
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<ReservationDto>(cancellationToken: ct);
    }

    // Nodig voor availability overlap checks.
    // Als je DAL deze endpoint nog niet heeft, voeg hem daar toe.
    public async Task<List<ReservationDto>> GetReservations(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<ReservationDto>>("api/reservations", ct);
        return list ?? new List<ReservationDto>();
    }

    // Assumptie: DAL heeft POST api/reservations dat dezelfde ReservationDto accepteert.
    public async Task<ReservationDto> CreateReservation(ReservationDto reservation, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/reservations", reservation, ct);
        resp.EnsureSuccessStatusCode();
        var created = await resp.Content.ReadFromJsonAsync<ReservationDto>(cancellationToken: ct);
        if (created is null) throw new InvalidOperationException("DAL returned empty response for reservation create.");
        return created;
    }

    public async Task<List<HotelRoomListItemDto>> GetHotelRooms(CancellationToken ct = default)
    {
        var rooms = await _http.GetFromJsonAsync<List<HotelRoomListItemDto>>("hotelroom", ct);
        return rooms ?? new List<HotelRoomListItemDto>();
    }

    public sealed class CampingApiClient
    {
        private readonly HttpClient _http;
        public CampingApiClient(HttpClient http) => _http = http;

        public async Task<List<CampingAccommodationDto>> GetAccommodaties(int id, CancellationToken ct = default)
        {
            var list = await _http.GetFromJsonAsync<List<CampingAccommodationDto>>($"api/Accommodatie/{id}", ct);
            return list ?? new List<CampingAccommodationDto>();
        }
    }

    public sealed class CampingAccommodationDto
    {
        public int AccommodatieID { get; set; }
        public int CampingID { get; set; }
    }
}
