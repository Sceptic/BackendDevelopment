using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public partial class Gite
    {
        public int GiteId { get; private set; }
        public int GiteNumber { get; private set; }
        public decimal GitePrice { get; private set; }
        public bool IsAvailable { get; private set; }
        public string GiteAddress { get; private set; } = null!;
        public int CapacityMin { get; private set; }
        public int CapacityMax { get; private set; }

        public GiteAmenities? Amenities { get; private set; }
        public ICollection<GiteBed> Beds { get; private set; } = new List<GiteBed>();
    }
}
