using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.IntegrationTests;

public sealed partial class ReservationApiSmokeTests : IClassFixture<ReservationApiFactory>, IAsyncLifetime
{
    private readonly ReservationApiFactory _factory;
    private readonly HttpClient _client;

    public ReservationApiSmokeTests(ReservationApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await RecreateDatabaseAsync(_factory.DatabaseName);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await DropDatabaseAsync(_factory.DatabaseName);
    }

    [Fact]
    public async Task Full_crud_smoke_test_hits_all_endpoints()
    {
        var createDto = new
        {
            accountId = 1,
            reservationStatus = "Created",
            paymentStatus = "Unpaid",
            reservationPrice = 0m,
            discount = 0m,
            touristTarif = 0m,
            reservationStart = DateTime.UtcNow.Date.AddDays(7),
            reservationEnd = DateTime.UtcNow.Date.AddDays(10),

            clients = new[]
            {
                new { firstName = "Test", lastName = "User", birthDate = new DateTime(1990, 1, 1) }
            },

            gites = new[]
            {
                new { giteId = 1, giteDiscount = 0m }
            },

            hotelrooms = Array.Empty<object>(),
            campings = Array.Empty<object>(),
            facilities = Array.Empty<object>(),
            vehicles = Array.Empty<object>(),
            restaurants = Array.Empty<object>(),
        };

        var postResp = await _client.PostAsJsonAsync("/reservation/post/CreateReservation", createDto);
        Assert.Equal(HttpStatusCode.Created, postResp.StatusCode);

        var allResp = await _client.GetAsync("/reservation/get/ReadReservations");
        Assert.Equal(HttpStatusCode.OK, allResp.StatusCode);

        int id;
        using (var doc = JsonDocument.Parse(await allResp.Content.ReadAsStringAsync()))
        {
            Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.True(doc.RootElement.GetArrayLength() >= 1);

            var first = doc.RootElement[0];
            id = first.GetProperty("reservationId").GetInt32();
            Assert.True(id > 0);
        }

        var getResp = await _client.GetAsync($"/reservation/get/ReadReservations/{id}");
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);

        var patchDto = new
        {
            reservationId = id,
            accountId = (int?)null,
            reservationStatus = (string?)null,
            paymentStatus = "Paid",
            reservationPrice = (decimal?)null,
            discount = (decimal?)null,
            touristTarif = (decimal?)null,
            reservationStart = (DateTime?)null,
            reservationEnd = (DateTime?)null,
            clients = (object?)null,
            gites = (object?)null,
            hotelrooms = (object?)null,
            campings = (object?)null,
            facilities = (object?)null,
            vehicles = (object?)null,
            restaurants = (object?)null
        };

        var patchResp = await _client.PatchAsJsonAsync($"/reservation/patch/UpdateReservation/{id}", patchDto);
        Assert.Equal(HttpStatusCode.OK, patchResp.StatusCode);

        var delResp = await _client.DeleteAsync($"/reservation/delete/DeleteReservation/{id}");
        Assert.Equal(HttpStatusCode.NoContent, delResp.StatusCode);

        var missing = await _client.GetAsync($"/reservation/get/ReadReservations/{id}");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }
}
