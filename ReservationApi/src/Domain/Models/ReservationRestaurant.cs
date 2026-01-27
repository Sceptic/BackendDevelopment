namespace Domain.Models;

public sealed partial class ReservationRestaurant
{
    public int ReservationRestaurantId { get; set; }
    public int ReservationId { get; set; }

    public int TableId { get; set; }
    public DateTime TableReservationStart { get; set; }
    public DateTime TableReservationEnd { get; set; }

    public decimal TableBill { get; set; }
    public decimal TableDiscount { get; set; }

    public Reservation Reservation { get; set; } = null!;
}
