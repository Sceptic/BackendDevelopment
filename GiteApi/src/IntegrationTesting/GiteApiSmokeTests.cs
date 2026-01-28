using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.IntegrationTests;

public sealed class GiteApiSmokeTests : IClassFixture<GiteApiFactory>, IAsyncLifetime
{
    private readonly GiteApiFactory _factory;
    private readonly HttpClient _client;

    public GiteApiSmokeTests(GiteApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Infrastructure.Persistence.GiteDbContext>();

        await db.Database.EnsureDeletedAsync();

        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Infrastructure.Persistence.GiteDbContext>();
        await db.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task Full_crud_smoke_test_hits_all_endpoints()
    {
        var createDto = new
        {
            giteNumber = 101,
            gitePrice = 123.45m,
            isAvailable = true,
            giteAddress = "Teststraat 1, 1234 AB",
            capacityMin = 1,
            capacityMax = 4,
            amenities = new
            {
                wifi = true,
                bath = false,
                shower = true,
                hairDryer = true,
                smallChild = false,
                toiletries = true,
                desk = true,
                chair = true,
                balcony = false,
                sofa = true,
                sofaBed = false,
                miniFridge = true,
                kettle = true,
                cuttlery = true,
                eatingArea = true,
                roomService = (bool?)null
            },
            beds = new[]
            {
                new { amount1PrBed = 2, amount2PrBed = 0, amount3PrBed = 0, bedSort = "Single" }
            }
        };

        var postResp = await _client.PostAsJsonAsync("/gite/post", createDto);
        Assert.Equal(HttpStatusCode.Created, postResp.StatusCode);

        var location = postResp.Headers.Location!.ToString();
        var idStr = location.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
        Assert.True(int.TryParse(idStr, out var id));
        Assert.True(id > 0);

        var getResp = await _client.GetAsync($"/gite/get/{id}");
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);

        var allResp = await _client.GetAsync("/gite/get/all");
        Assert.Equal(HttpStatusCode.OK, allResp.StatusCode);

        var updateDto = new
        {
            giteNumber = 101,
            gitePrice = 222.22m,
            isAvailable = false,
            giteAddress = "Nieuwe Straat 9, 9999 ZZ",
            capacityMin = 2,
            capacityMax = 6,
            amenities = new
            {
                wifi = true,
                bath = true,
                shower = true,
                hairDryer = false,
                smallChild = true,
                toiletries = true,
                desk = false,
                chair = true,
                balcony = true,
                sofa = true,
                sofaBed = true,
                miniFridge = true,
                kettle = true,
                cuttlery = true,
                eatingArea = true,
                roomService = true
            },
            beds = new[]
            {
                new { amount1PrBed = 0, amount2PrBed = 2, amount3PrBed = 0, bedSort = "Double" }
            }
        };

        var putResp = await _client.PutAsJsonAsync($"/gite/put/{id}", updateDto);
        Assert.Equal(HttpStatusCode.NoContent, putResp.StatusCode);

        var delResp = await _client.DeleteAsync($"/gite/delete/{id}");
        Assert.Equal(HttpStatusCode.NoContent, delResp.StatusCode);

        var missing = await _client.GetAsync($"/gite/get/{id}");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }
}
