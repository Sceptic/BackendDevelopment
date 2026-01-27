namespace Domain.Models;

public sealed partial class ReservationHotelroom
{
    public int ReservationId { get; set; }
    public int RoomId { get; set; }
    public decimal HotelroomDiscount { get; set; }

    public Reservation Reservation { get; set; } = null!;
}
