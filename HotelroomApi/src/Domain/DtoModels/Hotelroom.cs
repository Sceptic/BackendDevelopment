namespace Domain.Models;

public sealed class Hotelroom
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

    private Hotelroom(
        int roomId,
        int roomNumber,
        decimal? hotelroomPrice,
        bool? isAvailable,
        int? capacityMin,
        int? capacityMax)
    {
        RoomId = roomId;
        RoomNumber = roomNumber;
        HotelroomPrice = hotelroomPrice;
        IsAvailable = isAvailable;
        CapacityMin = capacityMin;
        CapacityMax = capacityMax;
    }

    public static Hotelroom Rehydrate(
        int roomId,
        int roomNumber,
        decimal? hotelroomPrice,
        bool? isAvailable,
        int? capacityMin,
        int? capacityMax)
        => new(roomId, roomNumber, hotelroomPrice, isAvailable, capacityMin, capacityMax);

    public void AttachAmenities(HotelroomAmenities amenities)
    {
        Amenities = amenities;
    }

    public void AddBed(HotelroomBed bed)
    {
        _beds.Add(bed);
    }
}
