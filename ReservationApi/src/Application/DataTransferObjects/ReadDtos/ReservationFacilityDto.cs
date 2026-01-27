namespace Application.DTOs.Reservations;

public sealed record ReservationFacilityDto
(
    int ReservationId,
    string Facility,
    decimal FacilityDiscount
);
