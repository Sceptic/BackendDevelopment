using WrapperApi.Contracts;
using WrapperApi.Wrapper;

namespace WrapperApi.Providers;

public sealed class GiteProvider : ICatalogProvider
{
    public SourceSystem Source => SourceSystem.Gite;
    private readonly GiteApiClient _gite;

    public GiteProvider(GiteApiClient gite) => _gite = gite;

    public async Task<IReadOnlyList<AccommodationCard>> GetCatalog(CancellationToken ct)
    {
        var gites = await _gite.GetAll(ct);

        return gites.Select(g => new AccommodationCard(
            Source: SourceSystem.Gite,
            AccommodationId: g.GiteId.ToString(),
            Name: $"Gite {g.GiteNumber}",
            Type: "Gite",
            CapacityMin: g.CapacityMin ?? 0,
            CapacityMax: g.CapacityMax ?? 0,
            PricePerNight: g.GitePrice ?? 0m,
            Currency: "EUR",
            Available: g.IsAvailable ?? false
        )).ToList();
    }

    public async Task<IReadOnlyList<AccommodationCard>> GetAvailability(AvailabilityQuery q, CancellationToken ct)
    {
        var items = (await GetCatalog(ct)).AsEnumerable();

        if (q.Guests is not null)
            items = items.Where(x => x.CapacityMin <= q.Guests && x.CapacityMax >= q.Guests);

        if (q.CapacityMin is not null) items = items.Where(x => x.CapacityMax >= q.CapacityMin);
        if (q.CapacityMax is not null) items = items.Where(x => x.CapacityMin <= q.CapacityMax);

        return items.ToList();
    }
}
