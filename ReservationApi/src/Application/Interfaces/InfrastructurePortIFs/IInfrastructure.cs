using Domain.Models;

namespace Application.Abstractions.Persistence;

public interface IReservationRepository
{
    Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Reservation?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default);

    Task<Reservation?> GetByIdTrackedAsync(int reservationId, CancellationToken cancellationToken = default);

    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);

    Task DeleteAsync(int reservationId, CancellationToken cancellationToken = default);
}
