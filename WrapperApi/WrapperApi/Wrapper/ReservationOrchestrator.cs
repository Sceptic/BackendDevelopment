namespace WrapperApi.Wrapper;

/// <summary>
/// Orchestrator die:
/// 1) data ophaalt uit Accounts/Hotel/Gite API
/// 2) availability bepaalt door overlap te checken tegen DAL-reservations
/// 3) reservatie aanmaakt in DAL als alles geldig is
/// </summary>
public sealed class ReservationOrchestrator
{
    private readonly AccountsApiClient _accounts;
    private readonly HotelApiClient _hotel;
    private readonly GiteApiClient _gite;
    private readonly DalApiClient _dal;

    public ReservationOrchestrator(AccountsApiClient accounts, HotelApiClient hotel, GiteApiClient gite, DalApiClient dal)
    {
        _accounts = accounts;
        _hotel = hotel;
        _gite = gite;
        _dal = dal;
    }

    // Overlap-regel: [start,end) overlap met [s2,e2) als start < e2 && end > s2
    private static bool Overlaps(DateTime start, DateTime end, DateTime s2, DateTime e2) =>
        start < e2 && end > s2;

    public async Task<List<HotelRoomListItemDto>> GetAvailableHotelRooms(DateTime start, DateTime end, int? capacityMin, int? capacityMax, CancellationToken ct = default)
    {
        var rooms = await _hotel.GetAll(ct);

        var filtered = rooms
            .Where(r => (r.IsAvailable ?? false))
            .Where(r => capacityMin is null || (r.CapacityMax ?? 0) >= capacityMin)
            .Where(r => capacityMax is null || (r.CapacityMin ?? 0) <= capacityMax)
            .ToList();

        if (filtered.Count == 0) return filtered;

        // Haal alle reservaties uit DAL en filter op overlap.
        // Optimisatie: maak in DAL een endpoint die meteen op daterange filtert.
        var reservations = await _dal.GetReservations(ct);

        var blockedRoomNumbers = new HashSet<int>(
            reservations
                .Where(r => Overlaps(start, end, r.ReservationStart, r.ReservationEnd))
                .SelectMany(r => r.Hotels ?? new List<ReservationHotelDto>())
                .Select(h => h.RoomNumber)
        );

        return filtered.Where(r => !blockedRoomNumbers.Contains(r.RoomNumber)).ToList();
    }

    public async Task<List<GiteListItemDto>> GetAvailableGites(DateTime start, DateTime end, int? capacityMin, int? capacityMax, CancellationToken ct = default)
    {
        var gites = await _gite.GetAll(ct);

        var filtered = gites
            .Where(g => (g.IsAvailable ?? false))
            .Where(g => capacityMin is null || (g.CapacityMax ?? 0) >= capacityMin)
            .Where(g => capacityMax is null || (g.CapacityMin ?? 0) <= capacityMax)
            .ToList();

        if (filtered.Count == 0) return filtered;

        var reservations = await _dal.GetReservations(ct);

        var blockedGiteNumbers = new HashSet<int>(
            reservations
                .Where(r => Overlaps(start, end, r.ReservationStart, r.ReservationEnd))
                .SelectMany(r => r.Gites ?? new List<ReservationGiteDto>())
                .Select(g => g.GiteNumber)
        );

        return filtered.Where(g => !blockedGiteNumbers.Contains(g.GiteNumber)).ToList();
    }

    public async Task<ReservationDto> CreateReservation(ReservationDto req, CancellationToken ct = default)
    {
        // Basisvalidatie
        if (req.AccountId <= 0) throw new ReservationValidationException("AccountId is required.");
        if (req.ReservationEnd <= req.ReservationStart) throw new ReservationValidationException("ReservationEnd must be after ReservationStart.");

        // 1) Account bestaat?
        if (!await _accounts.Exists(req.AccountId, ct))
            throw new ReservationValidationException($"Account {req.AccountId} not found.");

        // 2) Alle rooms/gites bestaan?
        foreach (var h in req.Hotels ?? new List<ReservationHotelDto>())
        {
            if (h.RoomNumber <= 0) throw new ReservationValidationException("Hotel RoomNumber must be > 0.");
            if (!await _hotel.ExistsRoom(h.RoomNumber, ct))
                throw new ReservationValidationException($"Hotel room {h.RoomNumber} not found.");
        }

        foreach (var g in req.Gites ?? new List<ReservationGiteDto>())
        {
            if (g.GiteNumber <= 0) throw new ReservationValidationException("GiteNumber must be > 0.");
            if (!await _gite.ExistsGite(g.GiteNumber, ct))
                throw new ReservationValidationException($"Gite {g.GiteNumber} not found.");
        }

        // 3) Overlap check tegen bestaande reservaties (DAL)
        // Als je dit schaalbaar wil maken: verplaats overlap-check naar DAL met query op daterange + resourceNumber.
        var existing = await _dal.GetReservations(ct);

        var start = req.ReservationStart;
        var end = req.ReservationEnd;

        // Check hotel rooms
        foreach (var h in req.Hotels ?? new List<ReservationHotelDto>())
        {
            var room = h.RoomNumber;
            var conflict = existing.Any(r =>
                Overlaps(start, end, r.ReservationStart, r.ReservationEnd) &&
                (r.Hotels?.Any(x => x.RoomNumber == room) ?? false));

            if (conflict)
                throw new ReservationConflictException($"Hotel room {room} is already reserved in this period.");
        }

        // Check gites
        foreach (var g in req.Gites ?? new List<ReservationGiteDto>())
        {
            var nr = g.GiteNumber;
            var conflict = existing.Any(r =>
                Overlaps(start, end, r.ReservationStart, r.ReservationEnd) &&
                (r.Gites?.Any(x => x.GiteNumber == nr) ?? false));

            if (conflict)
                throw new ReservationConflictException($"Gite {nr} is already reserved in this period.");
        }

        // 4) Opslaan via DAL
        // Safety: ReservationId vanuit client negeren (DAL moet id genereren)
        req.ReservationId = 0;
        foreach (var c in req.Clients ?? new List<ReservationClientDto>()) c.ReservationId = 0;
        foreach (var h in req.Hotels ?? new List<ReservationHotelDto>()) h.ReservationId = 0;
        foreach (var g in req.Gites ?? new List<ReservationGiteDto>()) g.ReservationId = 0;

        return await _dal.CreateReservation(req, ct);
    }
}
