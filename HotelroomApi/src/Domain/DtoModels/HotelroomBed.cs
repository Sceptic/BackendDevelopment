using System.Text.Json.Serialization;

namespace Domain.Models;

public sealed class HotelroomBed
{
    public int HotelroomBedId { get; private set; }
    public int RoomId { get; private set; }
    public int? Amount1PrBed { get; private set; }
    public int? Amount2PrBed { get; private set; }
    public int? Amount3PrBed { get; private set; }
    public string? BedSort { get; private set; }

    [JsonIgnore]
    public Hotelroom Room { get; private set; } = null!;

    private HotelroomBed() { }

    private HotelroomBed(
        int hotelroomBedId,
        int roomId,
        int? amount1PrBed,
        int? amount2PrBed,
        int? amount3PrBed,
        string? bedSort)
    {
        HotelroomBedId = hotelroomBedId;
        RoomId = roomId;
        Amount1PrBed = amount1PrBed;
        Amount2PrBed = amount2PrBed;
        Amount3PrBed = amount3PrBed;
        BedSort = bedSort;
    }

    public static HotelroomBed Rehydrate(
        int hotelroomBedId,
        int roomId,
        Hotelroom room,
        int? amount1PrBed,
        int? amount2PrBed,
        int? amount3PrBed,
        string? bedSort)
    {
        var bed = new HotelroomBed(
            hotelroomBedId,
            roomId,
            amount1PrBed,
            amount2PrBed,
            amount3PrBed,
            bedSort);

        bed.Room = room;
        return bed;
    }

    public void AttachTo(Hotelroom room)
    {
        Room = room;
        RoomId = room.RoomId;
    }
}
