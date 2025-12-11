using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class ReservationHotel
    {
        public int ReservationId { get; set; }
        public int RoomNumber { get; set; }

        public int HotelroomDiscount { get; set; }

        public Reservation Reservation { get; set; }
        public HotelRoom Room { get; set; }
    }
}
