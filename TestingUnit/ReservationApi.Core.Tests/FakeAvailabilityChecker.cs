using Application.Abstractions.Reservations;
using Domain.Models;

namespace ReservationApi.Core.Tests.Application;

internal sealed class FakeAvailabilityChecker : IReservationAvailabilityChecker
{
    public int Calls { get; private set; }
    public int? LastExcludeReservationId { get; private set; }
    public Reservation? LastReservation { get; private set; }

    public bool ThrowOnCall { get; set; }

    public Task EnsureNoConflictsAsync(Reservation reservation, int? excludeReservationId, CancellationToken ct = default)
    {
        Calls++;
        LastExcludeReservationId = excludeReservationId;
        LastReservation = reservation;

        if (ThrowOnCall)
            throw new InvalidOperationException("Conflict detected.");

        return Task.CompletedTask;
    }
}
