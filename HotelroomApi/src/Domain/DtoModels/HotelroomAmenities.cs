using System.Text.Json.Serialization;

namespace Domain.Models;

public sealed class HotelroomAmenities
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

    private HotelroomAmenities(
        bool wifi,
        bool bath,
        bool shower,
        bool hairdryer,
        bool smallchild,
        bool toiletries,
        bool desk,
        bool chair,
        bool balcony,
        bool sofa,
        bool sofabed,
        bool minifridge,
        bool kettle,
        bool cuttlery,
        bool eatingarea,
        bool roomservice)
    {
        Wifi = wifi;
        Bath = bath;
        Shower = shower;
        Hairdryer = hairdryer;
        Smallchild = smallchild;
        Toiletries = toiletries;
        Desk = desk;
        Chair = chair;
        Balcony = balcony;
        Sofa = sofa;
        Sofabed = sofabed;
        Minifridge = minifridge;
        Kettle = kettle;
        Cuttlery = cuttlery;
        Eatingarea = eatingarea;
        Roomservice = roomservice;
    }

    public static HotelroomAmenities Rehydrate(
        int roomId,
        Hotelroom room,
        bool? wifi,
        bool? bath,
        bool? shower,
        bool? hairdryer,
        bool? smallchild,
        bool? toiletries,
        bool? desk,
        bool? chair,
        bool? balcony,
        bool? sofa,
        bool? sofabed,
        bool? minifridge,
        bool? kettle,
        bool? cuttlery,
        bool? eatingarea,
        bool? roomservice)
    {
        var a = new HotelroomAmenities(
            wifi ?? false,
            bath ?? false,
            shower ?? false,
            hairdryer ?? false,
            smallchild ?? false,
            toiletries ?? false,
            desk ?? false,
            chair ?? false,
            balcony ?? false,
            sofa ?? false,
            sofabed ?? false,
            minifridge ?? false,
            kettle ?? false,
            cuttlery ?? false,
            eatingarea ?? false,
            roomservice ?? false
        );

        a.RoomId = roomId;
        a.Room = room;
        return a;
    }

    public void AttachTo(Hotelroom room)
    {
        Room = room;
        RoomId = room.RoomId;
    }
}
