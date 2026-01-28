using WrapperApi.Contracts;

namespace WrapperApi.Storage;

public sealed class ReservationDb : IReservationDb
{
    public Task<HashSet<(SourceSystem source, int id)>> GetReservedIds(
        DateTime start, DateTime end, SourceSystem? source, CancellationToken ct)
        => Task.FromResult(new HashSet<(SourceSystem, int)>());

    public Task<ReservationCreatedResponse?> TryGetByIdempotency(string key, CancellationToken ct)
        => Task.FromResult<ReservationCreatedResponse?>(null);

    public Task SaveIdempotency(string key, ReservationCreatedResponse created, CancellationToken ct)
        => Task.CompletedTask;

    public Task<int> CreateSingleReservation(CreateReservationRequest req, int itemId, CancellationToken ct)
        => Task.FromResult(1);

    public Task<object?> GetReservation(int reservationId, CancellationToken ct)
        => Task.FromResult<object?>(null);
}
