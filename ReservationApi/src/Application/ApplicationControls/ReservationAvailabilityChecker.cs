using Application.Abstractions.Persistence;
using Application.Abstractions.Reservations;
using Domain.ErrorHandling;
using Domain.Models;

namespace Application.Reservations;

public sealed class ReservationAvailabilityChecker : IReservationAvailabilityChecker
{
    private readonly IReservationConflictQueries _q;

    public ReservationAvailabilityChecker(IReservationConflictQueries q)
    {
        _q = q;
    }

    public async Task EnsureNoConflictsAsync(Reservation reservation, int? excludeReservationId, CancellationToken ct = default)
    {
        var start = reservation.ReservationStart;
        var end = reservation.ReservationEnd;

        foreach (var hr in reservation.Hotelrooms)
        {
            if (await _q.AnyRoomOverlapAsync(hr.RoomId, start, end, excludeReservationId, ct))
                throw new Domain.ErrorHandling.DomainValidationException($"Room {hr.RoomId} is already booked for the selected period.");
        }

        foreach (var g in reservation.Gites)
        {
            if (await _q.AnyGiteOverlapAsync(g.GiteId, start, end, excludeReservationId, ct))
                throw new Domain.ErrorHandling.DomainValidationException($"Gite {g.GiteId} is already booked for the selected period.");
        }

        foreach (var c in reservation.Campings)
        {
            if (await _q.AnyCampingOverlapAsync(c.CampingId, start, end, excludeReservationId, ct))
                throw new DomainValidationException($"CampingId {c.CampingId} is already booked for the selected period.");
        }

        foreach (var f in reservation.Facilities)
        {
            var facility = (f.Facility ?? string.Empty).Trim();
            if (await _q.AnyFacilityOverlapAsync(facility, start, end, excludeReservationId, ct))
                throw new DomainValidationException($"Facility '{facility}' is already booked for the selected period.");
        }

        foreach (var v in reservation.Vehicles)
        {
            var plate = (v.RegistrationPlate ?? string.Empty).Trim();
            if (await _q.AnyVehicleOverlapAsync(plate, start, end, excludeReservationId, ct))
                throw new DomainValidationException($"Vehicle '{plate}' is already booked for the selected period.");
        }

        foreach (var rr in reservation.Restaurants)
        {
            var excludeRestaurantId = rr.ReservationRestaurantId > 0 ? rr.ReservationRestaurantId : (int?)null;

            if (await _q.AnyTableOverlapAsync(
                    rr.TableId,
                    rr.TableReservationStart,
                    rr.TableReservationEnd,
                    excludeReservationId,
                    excludeRestaurantId,
                    ct))
            {
                throw new DomainValidationException(
                    $"Restaurant table {rr.TableId} is already booked for {rr.TableReservationStart:yyyy-MM-dd HH:mm}–{rr.TableReservationEnd:HH:mm}.");
            }
        }
    }
}
