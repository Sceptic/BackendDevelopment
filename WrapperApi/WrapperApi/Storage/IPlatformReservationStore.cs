using WrapperApi.Contracts;

namespace WrapperApi.Storage;

public interface IPlatformReservationStore
{
    Task<ReservationCreatedResponse?> TryGetByIdempotencyAsync(string key, CancellationToken ct);

    Task<ReservationCreatedResponse> CreateAsync(
        SourceSystem source,
        string sourceReservationId,
        string? idempotencyKey,
        CreateReservationRequest? cache,
        CancellationToken ct);

    Task<(SourceSystem Source, string SourceReservationId)?> TryGetMapAsync(int platformId, CancellationToken ct);
}
