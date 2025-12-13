using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public string ReservationStatus { get; set; }
        public string PaymentStatus { get; set; }

        public int Discount { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }

        public ICollection<ReservationClient> ReservationClient { get; set; }
        public ICollection<ReservationGite> ReservationGite { get; set; }
        public ICollection<ReservationHotel> ReservationHotel { get; set; }
    }
}
