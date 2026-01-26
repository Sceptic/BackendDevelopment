using WrapperApi.Contracts;

namespace WrapperApi.Storage;

public interface IReservationDb
{
    // geeft alle gereserveerde items in periode terug per source
    Task<HashSet<(SourceSystem source, int id)>> GetReservedIds(DateTime start, DateTime end, SourceSystem? source, CancellationToken ct);

    // idempotency
    Task<ReservationCreatedResponse?> TryGetByIdempotency(string key, CancellationToken ct);
    Task SaveIdempotency(string key, ReservationCreatedResponse created, CancellationToken ct);

    // create/read
    Task<int> CreateSingleReservation(CreateReservationRequest req, int itemId, CancellationToken ct);
    Task<object?> GetReservation(int reservationId, CancellationToken ct);
}
