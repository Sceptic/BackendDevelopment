using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class HotelRoomBed
    {
        public int RoomNumber { get; set; }

        public int Amount1PrBed { get; set; }
        public int Amount2PrBed { get; set; }
        public int Amount3PrBed { get; set; }

        public string BedSort { get; set; }

        public HotelRoom Room { get; set; }
    }
}
