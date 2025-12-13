using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class Gite
    {
        public int GiteNumber { get; set; }
        public decimal? GitePrice { get; set; }
        public bool? IsAvailable { get; set; }
        public string? GiteAddress { get; set; }
        public int? CapacityMin { get; set; }
        public int? CapacityMax { get; set; }

        public ICollection<ReservationGite> ReservationGites { get; set; }
    }
}
