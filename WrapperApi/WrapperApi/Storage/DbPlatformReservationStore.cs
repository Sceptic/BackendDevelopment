using Microsoft.EntityFrameworkCore;
using WrapperApi.Contracts;
using WrapperApi.Persistence;

namespace WrapperApi.Storage;

public sealed class DbPlatformReservationStore : IPlatformReservationStore
{
    private readonly PlatformDbContext _db;

    public DbPlatformReservationStore(PlatformDbContext db) => _db = db;

    public async Task<ReservationCreatedResponse?> TryGetByIdempotencyAsync(string key, CancellationToken ct)
    {
        var row = await _db.PlatformReservations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdempotencyKey == key, ct);

        return row is null
            ? null
            : new ReservationCreatedResponse(row.PlatformReservationId, row.Source, row.SourceReservationId);
    }

    public async Task<(SourceSystem Source, string SourceReservationId)?> TryGetMapAsync(int platformId, CancellationToken ct)
    {
        var row = await _db.PlatformReservations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PlatformReservationId == platformId, ct);

        return row is null ? null : (row.Source, row.SourceReservationId);
    }

    public async Task<ReservationCreatedResponse> CreateAsync(
        SourceSystem source,
        string sourceReservationId,
        string? idempotencyKey,
        CreateReservationRequest? cache,
        CancellationToken ct)
    {
        var entity = new PlatformReservationEntity
        {
            Source = source,
            SourceReservationId = sourceReservationId,
            IdempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey,
            CreatedAtUtc = DateTime.UtcNow,

            AccountId = cache?.AccountId,
            AccommodationId = cache?.AccommodationId,
            Start = cache?.Start,
            End = cache?.End,
            Guests = cache?.Guests
        };

        _db.PlatformReservations.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new ReservationCreatedResponse(entity.PlatformReservationId, entity.Source, entity.SourceReservationId);
    }
}
