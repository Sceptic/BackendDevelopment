namespace Domain.Models;

public sealed partial class ReservationGite
{
    public int ReservationId { get; set; }
    public int GiteId { get; set; }
    public decimal GiteDiscount { get; set; }

    public Reservation Reservation { get; set; } = null!;
}
