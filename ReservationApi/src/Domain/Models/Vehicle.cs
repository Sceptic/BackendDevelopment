namespace Domain.Models;

public sealed partial class Vehicle
{
    public int ReservationId { get; set; }
    public string RegistrationPlate { get; set; } = null!;

    public Reservation Reservation { get; set; } = null!;
}
