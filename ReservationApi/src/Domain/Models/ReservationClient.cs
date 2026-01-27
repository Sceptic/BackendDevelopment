namespace Domain.Models;

public sealed partial class ReservationClient
{
    public int ReservationId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public Reservation Reservation { get; set; } = null!;
}
