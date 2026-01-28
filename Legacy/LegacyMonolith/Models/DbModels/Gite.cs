using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyMonolith.Models
{
    public class Gite
    {
        public int GiteNumber { get; set; }
        public decimal? GitePrice { get; set; }
        public bool? IsAvailable { get; set; }
        public string? GiteAddress { get; set; }
        public int? CapacityMin { get; set; }
        public int? CapacityMax { get; set; }

        //This property doesn't serve a purpose, delete it and migrate the db again,
        //can't personally be bothered to do that rn.
        public ICollection<ReservationGite> ReservationGites { get; set; }
    }
}
