namespace Application.DTOs.Reservations;

public sealed record ReservationCampingDto
(
    int ReservationId,
    int CampingId,
    decimal CampingDiscount
);
