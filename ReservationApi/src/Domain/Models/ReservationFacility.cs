namespace Domain.Models;

public sealed partial class ReservationFacility
{
    public int ReservationId { get; set; }
    public string Facility { get; set; } = null!;
    public decimal FacilityDiscount { get; set; }

    public Reservation Reservation { get; set; } = null!;
}
