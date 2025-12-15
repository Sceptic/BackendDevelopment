using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class ReservationClient
    {
        public int ReservationId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }

        public Reservation Reservation { get; set; }
    }
}
