using Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class ReservationConflictQueries : IReservationConflictQueries
{
    private readonly ReservationDbContext _db;

    public ReservationConflictQueries(ReservationDbContext db)
    {
        _db = db;
    }

    public Task<bool> AnyRoomOverlapAsync(int roomId, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default)
    {
        var q =
            from rr in _db.ReservationHotelrooms
            join r in _db.Reservations on rr.ReservationId equals r.ReservationId
            where rr.RoomId == roomId
            where start < r.ReservationEnd && end > r.ReservationStart
            select r.ReservationId;

        if (excludeReservationId.HasValue)
            q = q.Where(id => id != excludeReservationId.Value);

        return q.AnyAsync(ct);
    }

    public Task<bool> AnyGiteOverlapAsync(int giteId, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default)
    {
        var q =
            from rg in _db.ReservationGites
            join r in _db.Reservations on rg.ReservationId equals r.ReservationId
            where rg.GiteId == giteId
            where start < r.ReservationEnd && end > r.ReservationStart
            select r.ReservationId;

        if (excludeReservationId.HasValue)
            q = q.Where(id => id != excludeReservationId.Value);

        return q.AnyAsync(ct);
    }

    public Task<bool> AnyCampingOverlapAsync(int campingId, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default)
    {
        var q =
            from rc in _db.ReservationCampings
            join r in _db.Reservations on rc.ReservationId equals r.ReservationId
            where rc.CampingId == campingId
            where start < r.ReservationEnd && end > r.ReservationStart
            select r.ReservationId;

        if (excludeReservationId.HasValue)
            q = q.Where(id => id != excludeReservationId.Value);

        return q.AnyAsync(ct);
    }

    public Task<bool> AnyFacilityOverlapAsync(string facility, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default)
    {
        facility = (facility ?? string.Empty).Trim();

        var q =
            from rf in _db.ReservationFacilities
            join r in _db.Reservations on rf.ReservationId equals r.ReservationId
            where rf.Facility.Trim() == facility
            where start < r.ReservationEnd && end > r.ReservationStart
            select r.ReservationId;

        if (excludeReservationId.HasValue)
            q = q.Where(id => id != excludeReservationId.Value);

        return q.AnyAsync(ct);
    }

    public Task<bool> AnyVehicleOverlapAsync(string registrationPlate, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default)
    {
        registrationPlate = (registrationPlate ?? string.Empty).Trim();

        var q =
            from v in _db.Vehicles
            join r in _db.Reservations on v.ReservationId equals r.ReservationId
            where v.RegistrationPlate.Trim() == registrationPlate
            where start < r.ReservationEnd && end > r.ReservationStart
            select r.ReservationId;

        if (excludeReservationId.HasValue)
            q = q.Where(id => id != excludeReservationId.Value);

        return q.AnyAsync(ct);
    }

    public Task<bool> AnyTableOverlapAsync(
        int tableId,
        DateTime tableStart,
        DateTime tableEnd,
        int? excludeReservationId,
        int? excludeReservationRestaurantId,
        CancellationToken ct = default)
    {
        var q =
            from tr in _db.ReservationRestaurants
            where tr.TableId == tableId
            where tableStart < tr.TableReservationEnd && tableEnd > tr.TableReservationStart
            select new { tr.ReservationId, tr.ReservationRestaurantId };

        if (excludeReservationId.HasValue)
            q = q.Where(x => x.ReservationId != excludeReservationId.Value);

        if (excludeReservationRestaurantId.HasValue)
            q = q.Where(x => x.ReservationRestaurantId != excludeReservationRestaurantId.Value);

        return q.AnyAsync(ct);
    }
}
