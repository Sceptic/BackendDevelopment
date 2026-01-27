namespace Application.DTOs.Reservations;

public sealed record ReservationRestaurantDto
(
    int ReservationRestaurantId,
    int ReservationId,
    int TableId,
    DateTime TableReservationStart,
    DateTime TableReservationEnd,
    decimal TableBill,
    decimal TableDiscount
);
