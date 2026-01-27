// Application/Abstractions/Reservations/IReservationExternalPolicy.cs
using Domain.Models;

namespace Application.Abstractions.Reservations;

public interface IReservationExternalPolicy
{
    Task ApplyAsync(Reservation reservation, CancellationToken ct);
}