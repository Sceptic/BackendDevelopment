using Application.Abstractions.Persistence;
using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Domain.Models;

namespace Application.Reservations;

public sealed partial class ReservationCommandService : IReservationCommandService
{
    public async Task<bool> DeleteAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdAsync(reservationId, cancellationToken);
        if (existing is null) return false;

        await _repo.DeleteAsync(reservationId, cancellationToken);
        return true;
    }
}
