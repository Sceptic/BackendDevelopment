namespace LegacyMonolith.Models
{
    public sealed class HotelRoomDto
    {
        public int RoomNumber { get; set; }
        public decimal? HotelroomPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public int? CapacityMin { get; set; }
        public int? CapacityMax { get; set; }

        public HotelRoomBedDto Bed { get; set; }
        public HotelRoomAmenitiesDto Amenities { get; set; }
    }


    public sealed class HotelRoomBedDto
    {
        public int? Amount1PrBed { get; set; }
        public int? Amount2PrBed { get; set; }
        public int? Amount3PrBed { get; set; }
        public string? BedSort { get; set; }
    }


    public sealed class HotelRoomAmenitiesDto
    {
        public bool? Wifi { get; set; }
        public bool? Bath { get; set; }
        public bool? Shower { get; set; }
        public bool? Hairdryer { get; set; }
        public bool? Smallchild { get; set; }
        public bool? Toiletries { get; set; }
        public bool? Desk { get; set; }
        public bool? Chair { get; set; }
        public bool? Balcony { get; set; }
        public bool? Sofa { get; set; }
        public bool? Sofabed { get; set; }
        public bool? Minifridge { get; set; }
        public bool? Kettle { get; set; }
        public bool? Cuttlery { get; set; }
        public bool? Eatingarea { get; set; }
        public bool? Roomservice { get; set; }
    }
}
