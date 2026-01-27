namespace Application.DTOs.Reservations;

public sealed record ReservationSummaryDto
(
    int ReservationId,
    int AccountId,
    string ReservationStatus,
    string PaymentStatus,
    decimal ReservationPrice,
    decimal Discount,
    decimal TouristTarif,
    DateTime ReservationStart,
    DateTime ReservationEnd
);
