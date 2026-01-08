using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyMonolith.Models
{
    public class HotelRoomAmenities
    {
        public int RoomNumber { get; set; }

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

        public HotelRoom Room { get; set; }
    }
}
