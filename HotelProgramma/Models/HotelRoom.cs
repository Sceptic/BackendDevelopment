using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class HotelRoom
    {
        public int RoomNumber { get; set; }
        public int HotelroomPrice { get; set; }
        public bool IsAvailable { get; set; }

        public HotelRoomBed Bed { get; set; }
        public HotelRoomAmenities Amenities { get; set; }
        public ICollection<ReservationHotel> ReservationHotels { get; set; }
    }
}
