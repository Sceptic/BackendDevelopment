namespace Domain.Models;

public sealed partial class Reservation
{
    public int ReservationId { get; set; }
    public int AccountId { get; set; }

    public string ReservationStatus { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;

    public decimal ReservationPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TouristTarif { get; set; }

    public DateTime ReservationStart { get; set; }
    public DateTime ReservationEnd { get; set; }

    public ICollection<ReservationClient> Clients { get; set; } = new List<ReservationClient>();
    public ICollection<ReservationGite> Gites { get; set; } = new List<ReservationGite>();
    public ICollection<ReservationHotelroom> Hotelrooms { get; set; } = new List<ReservationHotelroom>();
    public ICollection<ReservationCamping> Campings { get; set; } = new List<ReservationCamping>();
    public ICollection<ReservationRestaurant> Restaurants { get; set; } = new List<ReservationRestaurant>();
    public ICollection<ReservationFacility> Facilities { get; set; } = new List<ReservationFacility>();
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
