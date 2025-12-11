using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class HotelRoom
    {
        public int RoomNumber { get; set; }
        public decimal HotelroomPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int CapacityMin { get; set; }
        public int CapacityMax { get; set; }

        public HotelRoomBed Bed { get; set; }
        public HotelRoomAmenities Amenities { get; set; }
        public ICollection<ReservationHotel> ReservationHotels { get; set; }
    }
}
