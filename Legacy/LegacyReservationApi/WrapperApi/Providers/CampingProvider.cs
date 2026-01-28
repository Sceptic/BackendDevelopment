using WrapperApi.Contracts;
using WrapperApi.Wrapper;
using static WrapperApi.Wrapper.DalApiClient;

namespace WrapperApi.Providers;

public sealed class CampingProvider : ICatalogProvider
{
    public SourceSystem Source => SourceSystem.Camping;
    private readonly CampingApiClient _camping;

    public CampingProvider(CampingApiClient camping) => _camping = camping;

    public async Task<IReadOnlyList<AccommodationCard>> GetCatalog(CancellationToken ct)
    {
        // jouw endpoint wil een {id}; jij krijgt in praktijk een lijst terug
        var acc = await _camping.GetAccommodaties(0, ct);

        // Camping API geeft geen prijs/capacity/isAvailable: je kunt alleen “bestaat” tonen.
        return acc.Select(a => new AccommodationCard(
            Source: SourceSystem.Camping,
            AccommodationId: a.AccommodatieID.ToString(),  // let op mismatch met reservation_db (zie punt 0)
            Name: $"Camping accommodation {a.AccommodatieID}",
            Type: "Camping",
            CapacityMin: 0,
            CapacityMax: 0,
            PricePerNight: 0m,
            Currency: "EUR",
            Available: true
        )).ToList();
    }

    public Task<IReadOnlyList<AccommodationCard>> GetAvailability(AvailabilityQuery q, CancellationToken ct)
        => GetCatalog(ct);
}
