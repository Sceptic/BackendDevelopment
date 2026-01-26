using WrapperApi.Contracts;
using WrapperApi.Exceptions;
using WrapperApi.Providers;
using WrapperApi.Storage;

namespace WrapperApi.Orchestration;

public sealed class ReservationOrchestrator
{
    private readonly IReadOnlyList<ICatalogProvider> _providers;
    private readonly IReservationDb _db; // nieuwe interface voor reservation_db

    public ReservationOrchestrator(IEnumerable<ICatalogProvider> providers, IReservationDb db)
    {
        _providers = providers.ToList();
        _db = db;
    }

    public async Task<IReadOnlyList<AccommodationCard>> GetAvailability(AvailabilityQuery q, CancellationToken ct)
    {
        if (q.End <= q.Start)
            throw new ReservationValidationException("end must be after start");

        var targets = q.Source is null
            ? _providers
            : _providers.Where(p => p.Source == q.Source.Value).ToList();

        // 1) live catalog/availability uit bronnen
        var results = await Task.WhenAll(targets.Select(p => p.GetAvailability(q, ct)));
        var items = results.SelectMany(x => x)
            .Where(x => x.Available) // bron “hard unavailable” filter
            .ToList();

        // 2) reserved uit reservation_db (overlap)
        var reserved = await _db.GetReservedIds(q.Start, q.End, q.Source, ct);

        // 3) filter items die al reserved zijn
        items = items.Where(i =>
        {
            if (!int.TryParse(i.AccommodationId, out var id)) return false;
            return !reserved.Contains((i.Source, id));
        }).ToList();

        return items
            .OrderBy(x => x.Source)
            .ThenBy(x => x.Type)
            .ThenBy(x => x.PricePerNight)
            .ToList();
    }

    public async Task<ReservationCreatedResponse> CreateReservation(CreateReservationRequest req, CancellationToken ct)
    {
        if (req.End <= req.Start)
            throw new ReservationValidationException("end must be after start");

        if (!int.TryParse(req.AccommodationId, out var id) || id <= 0)
            throw new ReservationValidationException("AccommodationId must be a positive integer");

        // idempotency: als al bestaat in DB, return die
        if (!string.IsNullOrWhiteSpace(req.IdempotencyKey))
        {
            var existing = await _db.TryGetByIdempotency(req.IdempotencyKey, ct);
            if (existing is not null) return existing;
        }

        // conflict + insert in reservation_db
        var reservationId = await _db.CreateSingleReservation(req, id, ct);

        // SourceReservationId is niet echt van bron, dus gebruik reservationId als string
        var created = new ReservationCreatedResponse(
            PlatformReservationId: reservationId,
            Source: req.Source,
            SourceReservationId: reservationId.ToString()
        );

        if (!string.IsNullOrWhiteSpace(req.IdempotencyKey))
            await _db.SaveIdempotency(req.IdempotencyKey!, created, ct);

        return created;
    }

    public Task<object?> GetReservation(int platformId, CancellationToken ct)
        => _db.GetReservation(platformId, ct);
}
