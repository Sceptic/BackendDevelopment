using System;
using System.Collections.Generic;
using System.Text;

namespace HotelProgramma.Models
{
    public class Gite
    {
        public int GiteNumber { get; set; }
        public int GitePrice { get; set; }
        public bool IsAvailable { get; set; }
        public string GiteAddress { get; set; }
        public int Capacity { get; set; }

        public ICollection<ReservationGite> ReservationGites { get; set; }
    }
}
