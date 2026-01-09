using System.Text.Json.Serialization;

namespace Domain.Models;

public sealed partial class HotelroomAmenities
{
    public int RoomId { get; private set; }
    public bool Wifi { get; private set; }
    public bool Bath { get; private set; }
    public bool Shower { get; private set; }
    public bool Hairdryer { get; private set; }
    public bool Smallchild { get; private set; }
    public bool Toiletries { get; private set; }
    public bool Desk { get; private set; }
    public bool Chair { get; private set; }
    public bool Balcony { get; private set; }
    public bool Sofa { get; private set; }
    public bool Sofabed { get; private set; }
    public bool Minifridge { get; private set; }
    public bool Kettle { get; private set; }
    public bool Cuttlery { get; private set; }
    public bool Eatingarea { get; private set; }
    public bool Roomservice { get; private set; }

    [JsonIgnore]
    public Hotelroom Room { get; private set; } = null!;

    private HotelroomAmenities() { }
}
