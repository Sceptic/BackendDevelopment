using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public partial class GiteAmenities
    {
        public int GiteId { get; private set; }
        public bool? Wifi { get; private set; }
        public bool? Bath { get; private set; }
        public bool? Shower { get; private set; }
        public bool? HairDryer { get; private set; }
        public bool? SmallChild { get; private set; }
        public bool? Toiletries { get; private set; }
        public bool? Desk { get; private set; }
        public bool? Chair { get; private set; }
        public bool? Balcony { get; private set; }
        public bool? Sofa { get; private set; }
        public bool? SofaBed { get; private set; }
        public bool? MiniFridge { get; private set; }
        public bool? Kettle { get; private set; }
        public bool? Cuttlery { get; private set; }
        public bool? EatingArea { get; private set; }
        public bool? RoomService { get; private set; }

        [JsonIgnore]
        public Gite Gite { get; private set; } = null!;
    }
}
