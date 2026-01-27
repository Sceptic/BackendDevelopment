using Application.DTOs.Reservations;

namespace Application.Abstractions.Reservations;

public interface IReservationReadService
{
    Task<IReadOnlyList<ReservationSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ReservationDto?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default);
}
