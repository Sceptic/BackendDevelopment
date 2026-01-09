namespace Domain.Models;

public sealed partial class Hotelroom
{
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
