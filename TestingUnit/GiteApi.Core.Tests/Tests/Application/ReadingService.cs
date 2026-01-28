using Application.Abstractions;
using Application.Gites.ReadQueries;
using Domain.Models;
using Domain.Specs;

namespace GiteApi.Core.Tests.Application;

public sealed partial class GiteApplicationTests
{
    [Fact] //UT-GITE-APP-READ-001
    public async Task ReadingService_delegates_to_repository()
    {
        var repo = new FakeGiteRepository();
        var gite = Gite.Create(1, 10m, true, "A", 1, 2,
            new GiteAmenitiesSpec(true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false),
            new[] { new GiteBedSpec(1, 0, 0, "single") });
        repo.Seed(7, gite);

        var service = new GiteReadingService(repo);

        var found = await service.GetByIdAsync(7, CancellationToken.None);

        Assert.Equal(1, repo.GetByIdCalls);
        Assert.NotNull(found);
        Assert.Equal(7, found!.GiteId);

        var all = await service.GetAllAsync(CancellationToken.None);
        Assert.Equal(1, repo.GetAllCalls);
        Assert.Single(all);
    }
}