using Application.DTOs.Reservations;

namespace Application.Abstractions.Reservations;

public interface IReservationCommandService
{
    Task<ReservationDto> CreateAsync(CreateReservationRequestDto request, CancellationToken cancellationToken = default);
    Task<ReservationDto?> PatchAsync(PatchReservationRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int reservationId, CancellationToken cancellationToken = default);
}
