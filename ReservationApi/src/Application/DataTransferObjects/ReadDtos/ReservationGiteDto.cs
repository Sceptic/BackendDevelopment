namespace Application.DTOs.Reservations;

public sealed record ReservationGiteDto
(
    int ReservationId,
    int GiteId,
    decimal GiteDiscount
);
