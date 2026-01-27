namespace Application.DTOs.Reservations;

public sealed record ReservationHotelroomDto
(
    int ReservationId,
    int RoomId,
    decimal HotelroomDiscount
);
