using Domain.Models;

namespace Application.Abstractions.Reservations;

public interface IReservationAvailabilityChecker
{
    Task EnsureNoConflictsAsync(Reservation reservation, int? excludeReservationId, CancellationToken ct = default);
}
