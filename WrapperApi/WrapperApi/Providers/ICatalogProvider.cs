using WrapperApi.Contracts;

namespace WrapperApi.Providers;

public interface ICatalogProvider
{
    SourceSystem Source { get; }

    Task<IReadOnlyList<AccommodationCard>> GetCatalog(CancellationToken ct);

    // optioneel: dit kan gewoon GetCatalog + filters zijn
    Task<IReadOnlyList<AccommodationCard>> GetAvailability(AvailabilityQuery q, CancellationToken ct);
}
