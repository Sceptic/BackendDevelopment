using System.Net;
using System.Text.Json;

namespace FullSystemIntegrationTesting;

public sealed class FullSystemIntegrationTests
{
    private static readonly JsonSerializerOptions Camel = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    [Fact]
    public async Task IT_HOTELROOM_API_READ_001()
    {
        using var client = Http.CreateClient(TestConfig.HotelroomApiBaseUrl);

        // GET /hotelroom
        var list = await client.GetAsync("/hotelroom");
        Http.AssertStatus(HttpStatusCode.OK, list);

        using (var json = await Http.ReadJsonAsync(list))
        {
            if (json.RootElement.ValueKind != JsonValueKind.Array)
                throw new Xunit.Sdk.XunitException("Expected GET /hotelroom to return a JSON array.");

            if (json.RootElement.GetArrayLength() < 1)
                throw new Xunit.Sdk.XunitException("Expected GET /hotelroom array length to be >= 1.");
        }

        // GET /hotelroom/1
        var existing = await client.GetAsync("/hotelroom/1");
        Http.AssertStatus(HttpStatusCode.OK, existing);

        // GET /hotelroom/999999
        var missing = await client.GetAsync("/hotelroom/999999");
        Http.AssertStatus(HttpStatusCode.NotFound, missing);
    }

    [Fact]
    public async Task IT_GITE_API_CRUD_001()
    {
        using var client = Http.CreateClient(TestConfig.GiteApiBaseUrl);

        var createBody = new
        {
            giteNumber = 123,
            gitePrice = 150m,
            isAvailable = true,
            giteAddress = "Teststraat 1",
            capacityMin = 1,
            capacityMax = 4,
            amenities = new
            {
                wifi = true,
                bath = true,
                shower = true
            },
            beds = new[]
    {
        new { amount1PrBed = 1, amount2PrBed = 0, amount3PrBed = 0, bedSort = "Single" }
    }
        };

        var updateBody = new
        {
            giteNumber = 123,
            gitePrice = 175m,
            isAvailable = true,
            giteAddress = "Teststraat 1B",
            capacityMin = 1,
            capacityMax = 5,
            amenities = new
            {
                wifi = true,
                bath = true,
                shower = true,
                kettle = true
            },
            beds = new[]
            {
                new { amount1PrBed = 1, amount2PrBed = 1, amount3PrBed = 0, bedSort = "Mixed" }
            }
        };

        var createJson = JsonSerializer.Serialize(createBody, Camel);
        var updateJson = JsonSerializer.Serialize(updateBody, Camel);

        var created = await client.PostJsonStringAsync("/gite/post", createJson);
        Http.AssertStatus(HttpStatusCode.Created, created);

        var id = Http.ExtractTrailingIntIdFromLocation(created);

        var get = await client.GetAsync($"/gite/get/{id}");
        Http.AssertStatus(HttpStatusCode.OK, get);

        var all = await client.GetAsync("/gite/get/all");
        Http.AssertStatus(HttpStatusCode.OK, all);

        var put = await client.PutJsonStringAsync($"/gite/put/{id}", updateJson);
        Http.AssertStatus(HttpStatusCode.NoContent, put);

        var del = await client.DeleteAsync($"/gite/delete/{id}");
        Http.AssertStatus(HttpStatusCode.NoContent, del);

        var after = await client.GetAsync($"/gite/get/{id}");
        Http.AssertStatus(HttpStatusCode.NotFound, after);
    }

    [Fact]
    public async Task IT_RESERVATION_API_CRUD_001()
    {
        using var client = Http.CreateClient(TestConfig.ReservationApiBaseUrl);

        var start = DateTime.UtcNow.Date.AddDays(90);
        var end = start.AddDays(3);

        var createBody = new
        {
            accountId = 1,
            reservationStatus = "Created",
            paymentStatus = "Unpaid",
            reservationPrice = 0m,
            discount = 0m,
            touristTarif = 0m,
            reservationStart = start,
            reservationEnd = end,
            clients = new[]
            {
            new { firstName = "Test", lastName = "Client", birthDate = new DateTime(1990, 1, 1) }
        },
            gites = new[]
            {
            new { giteId = TestConfig.ReservationTestGiteId, giteDiscount = 0m }
        },
            hotelrooms = Array.Empty<object>(),
            campings = Array.Empty<object>(),
            facilities = Array.Empty<object>(),
            vehicles = Array.Empty<object>(),
            restaurants = Array.Empty<object>()
        };

        var createJson = JsonSerializer.Serialize(createBody, Camel);

        // POST /reservation/post/CreateReservation
        var created = await client.PostJsonStringAsync("/reservation/post/CreateReservation", createJson);
        Http.AssertStatus(HttpStatusCode.Created, created);

        // id from Location: /reservation/get/ReadReservations/{id}
        var id = Http.ExtractTrailingIntIdFromLocation(created);

        // GET list
        var all = await client.GetAsync("/reservation/get/ReadReservations");
        Http.AssertStatus(HttpStatusCode.OK, all);
        using (var json = await Http.ReadJsonAsync(all))
        {
            if (json.RootElement.ValueKind != JsonValueKind.Array)
                throw new Xunit.Sdk.XunitException("Expected GET /reservation/get/ReadReservations to return a JSON array.");
            if (json.RootElement.GetArrayLength() < 1)
                throw new Xunit.Sdk.XunitException("Expected GET /reservation/get/ReadReservations array length to be >= 1.");
        }

        // GET by id
        var get = await client.GetAsync($"/reservation/get/ReadReservations/{id}");
        Http.AssertStatus(HttpStatusCode.OK, get);

        // PATCH (must include ReservationId and it must match route id)
        var patchBody = new
        {
            reservationId = id,
            paymentStatus = "Paid"
        };
        var patchJson = JsonSerializer.Serialize(patchBody, Camel);

        var patched = await client.PatchJsonStringAsync($"/reservation/patch/UpdateReservation/{id}", patchJson);
        Http.AssertStatus(HttpStatusCode.OK, patched);

        var afterPatch = await client.GetAsync($"/reservation/get/ReadReservations/{id}");
        Http.AssertStatus(HttpStatusCode.OK, afterPatch);

        using (var json = await Http.ReadJsonAsync(afterPatch))
        {
            if (json.RootElement.TryGetProperty("paymentStatus", out var ps))
            {
                if (!string.Equals(ps.GetString(), "Paid", StringComparison.OrdinalIgnoreCase))
                    throw new Xunit.Sdk.XunitException("Expected paymentStatus to be 'Paid' after patch.");
            }
            else
            {
                throw new Xunit.Sdk.XunitException("Expected GET-by-id response to contain paymentStatus.");
            }
        }

        // DELETE
        var del = await client.DeleteAsync($"/reservation/delete/DeleteReservation/{id}");
        Http.AssertStatus(HttpStatusCode.NoContent, del);

        // GET after delete -> 404
        var after = await client.GetAsync($"/reservation/get/ReadReservations/{id}");
        Http.AssertStatus(HttpStatusCode.NotFound, after);
    }
}
