using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyMonolith.Models
{
    public class ReservationGite
    {
        public int ReservationId { get; set; }
        public int GiteNumber { get; set; }

        public int GiteDiscount { get; set; }

        public Reservation Reservation { get; set; }
        public Gite Gite { get; set; }
    }
}
