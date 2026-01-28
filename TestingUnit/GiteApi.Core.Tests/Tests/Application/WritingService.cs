using Application.DtoModels;
using Application.Gites.WriteQueries;
using Domain.Models;
using Domain.Specs;
using GiteApi.Core.Tests.Application;

namespace GiteApi.Core.Tests.Application;

//Helper method, creeërt een volledig Gite om meteen in een test-case te zetten i.p.v. elke
//keer eentje handmatig te maken. Je kan makkelijk specifieke waardes overwriten indien nodig.
public sealed partial class GiteApplicationTests
{
    private static GiteDto MakeDto(
        int number = 1,
        decimal price = 10m,
        bool isAvailable = true,
        string address = "Addr",
        int capMin = 1,
        int capMax = 2,
        int beds = 1)
    {
        return new GiteDto
        {
            GiteNumber = number,
            GitePrice = price,
            IsAvailable = isAvailable,
            GiteAddress = address,
            CapacityMin = capMin,
            CapacityMax = capMax,
            Amenities = new GiteAmenitiesDto
            {
                Wifi = true,
                Bath = false,
                Shower = true,
                HairDryer = false,
                SmallChild = false,
                Toiletries = false,
                Desk = false,
                Chair = false,
                Balcony = false,
                Sofa = false,
                SofaBed = false,
                MiniFridge = false,
                Kettle = false,
                Cuttlery = false,
                EatingArea = false,
                RoomService = false
            },
            Beds = Enumerable.Range(0, beds).Select(i => new GiteBedDto
            {
                Amount1PrBed = 1,
                Amount2PrBed = 0,
                Amount3PrBed = 0,
                BedSort = "single"
            }).ToList()
        };
    }
}

public sealed partial class GiteApplicationTests
{
    [Fact] //UT-GITE-APP-WRITE-CREATE-002
    public async Task WritingService_CreateAsync_adds_entity_and_returns_assigned_id()
    {
        var repo = new FakeGiteRepository();
        var service = new GiteWritingService(repo);

        var id = await service.CreateAsync(MakeDto(number: 5, price: 12.34m, beds: 1), CancellationToken.None);

        Assert.Equal(1, repo.AddCalls);
        Assert.NotNull(repo.LastAdded);
        Assert.Equal(5, repo.LastAdded!.GiteNumber);
        Assert.Equal(12.34m, repo.LastAdded!.GitePrice);
        Assert.True(id > 0);
        Assert.Equal(id, repo.LastAdded!.GiteId);
    }

    [Fact] //UT-GITE-APP-WRITE-UPDATE-003
    public async Task WritingService_UpdateAsync_throws_when_not_found()
    {
        var repo = new FakeGiteRepository();
        var service = new GiteWritingService(repo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateAsync(123, MakeDto(), CancellationToken.None));
    }

    [Fact] //UT-GITE-APP-WRITE-UPDATE-004
    public async Task WritingService_UpdateAsync_mutates_and_persists()
    {
        var repo = new FakeGiteRepository();
        var existing = Gite.Create(1, 10m, true, "Old", 1, 2,
            new GiteAmenitiesSpec(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false),
            new[] { new GiteBedSpec(1, 0, 0, "single") });
        repo.Seed(9, existing);

        var service = new GiteWritingService(repo);

        var dto = MakeDto(number: 999, price: 99.99m, isAvailable: false, address: "NewAddr", capMin: 2, capMax: 5, beds: 2);

        await service.UpdateAsync(9, dto, CancellationToken.None);

        Assert.Equal(1, repo.UpdateCalls);
        Assert.NotNull(repo.LastUpdated);
        Assert.Same(existing, repo.LastUpdated);

        Assert.Equal(99.99m, existing.GitePrice);
        Assert.False(existing.IsAvailable);
        Assert.Equal("NewAddr", existing.GiteAddress);
        Assert.Equal(2, existing.CapacityMin);
        Assert.Equal(5, existing.CapacityMax);
        Assert.Equal(2, existing.Beds.Count);
        Assert.True(existing.Amenities!.Wifi);
    }

    [Fact] //UT-GITE-APP-WRITE-DELETE-005
    public async Task WritingService_DeleteAsync_deletes_when_found()
    {
        var repo = new FakeGiteRepository();
        var existing = Gite.Create(1, 10m, true, "A", 1, 2,
            new GiteAmenitiesSpec(true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false),
            new[] { new GiteBedSpec(1, 0, 0, "single") });
        repo.Seed(3, existing);

        var service = new GiteWritingService(repo);

        await service.DeleteAsync(3, CancellationToken.None);

        Assert.Equal(1, repo.DeleteCalls);
        Assert.Same(existing, repo.LastDeleted);
        Assert.Null(await repo.GetByIdAsync(3));
    }
}

