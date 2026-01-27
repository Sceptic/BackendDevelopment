namespace Domain.Models;

public sealed partial class ReservationCamping
{
    public int ReservationId { get; set; }
    public int CampingId { get; set; }
    public decimal CampingDiscount { get; set; }

    public Reservation Reservation { get; set; } = null!;
}
