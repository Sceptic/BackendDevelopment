using System.Text.Json.Serialization;

namespace Domain.Models;

public sealed partial class HotelroomBed
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
}
