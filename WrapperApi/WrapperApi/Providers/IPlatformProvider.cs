using WrapperApi.Contracts;

namespace WrapperApi.Providers;

public interface IPlatformProvider
{
    SourceSystem Source { get; }
    Task<IReadOnlyList<AccommodationCard>> GetCatalog(CancellationToken ct);
    Task<IReadOnlyList<AccommodationCard>> GetAvailability(AvailabilityQuery q, CancellationToken ct);
    Task<string> CreateReservation(CreateReservationRequest req, CancellationToken ct);
    Task<object?> GetReservation(string sourceReservationId, CancellationToken ct);
}