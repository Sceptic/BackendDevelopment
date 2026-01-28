using System.Net;
using System.Net.Http.Json;
using Application.DTOs.Reservations;
using Xunit;

namespace ReservationApi.IntegrationTests;

public sealed class ReservationApiIntegrationTests : IClassFixture<ReservationApiFactory>
{
    private readonly HttpClient _client;

    public ReservationApiIntegrationTests(ReservationApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact] //IT-RES-API-CRUD-001
    public async Task Create_then_GetById_roundtrip()
    {
        var start = new DateTime(2026, 06, 01, 14, 00, 00);
        var end = new DateTime(2026, 06, 10, 10, 00, 00);

        var req = new CreateReservationRequestDto(
            AccountId: 1001,
            ReservationStatus: "CONFIRMED",
            PaymentStatus: "PAID",
            ReservationPrice: 1200m,
            Discount: 0.10m,
            TouristTarif: 0.05m,
            ReservationStart: start,
            ReservationEnd: end,
            Clients: new[] { new CreateReservationClientDto("Anna", "Peeters", new DateTime(1985, 03, 22)) },
            Gites: Array.Empty<CreateReservationGiteDto>(),
            Hotelrooms: Array.Empty<CreateReservationHotelroomDto>(),
            Campings: Array.Empty<CreateReservationCampingDto>(),
            Facilities: Array.Empty<CreateReservationFacilityDto>(),
            Vehicles: Array.Empty<CreateVehicleDto>(),
            Restaurants: Array.Empty<CreateReservationRestaurantDto>()
        );

        var post = await _client.PostAsJsonAsync("/create", req);
        Assert.Equal(HttpStatusCode.OK, post.StatusCode);

        var created = await post.Content.ReadFromJsonAsync<ReservationDto>();
        Assert.NotNull(created);
        Assert.True(created!.ReservationId > 0);

        var get = await _client.GetAsync($"/get/{created.ReservationId}");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);

        var fetched = await get.Content.ReadFromJsonAsync<ReservationDto>();
        Assert.NotNull(fetched);
        Assert.Equal(created.ReservationId, fetched!.ReservationId);
        Assert.Equal(1001, fetched.AccountId);
    }
}
