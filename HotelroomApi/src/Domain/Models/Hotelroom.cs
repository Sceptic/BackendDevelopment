namespace Domain.Models;

public sealed partial class Hotelroom
{
    public int RoomId { get; private set; }
    public int RoomNumber { get; private set; }
    public decimal? HotelroomPrice { get; private set; }
    public bool? IsAvailable { get; private set; }
    public int? CapacityMin { get; private set; }
    public int? CapacityMax { get; private set; }

    private readonly List<HotelroomBed> _beds = new();
    public IReadOnlyCollection<HotelroomBed> Beds => _beds.AsReadOnly();

    public HotelroomAmenities? Amenities { get; private set; }

    private Hotelroom() { }
}
