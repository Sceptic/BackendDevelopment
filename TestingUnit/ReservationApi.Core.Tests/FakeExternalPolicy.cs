using Application.Abstractions.Reservations;
using Domain.Models;

namespace ReservationApi.Core.Tests.Application;

internal sealed class FakeExternalPolicy : IReservationExternalPolicy
{
    public int Calls { get; private set; }
    public Reservation? LastReservation { get; private set; }
    public bool MutateOnCall { get; set; }

    public Task ApplyAsync(Reservation reservation, CancellationToken ct)
    {
        Calls++;
        LastReservation = reservation;

        if (MutateOnCall && reservation.Discount == 0m)
            reservation.Discount = 0.10m;

        return Task.CompletedTask;
    }
}
