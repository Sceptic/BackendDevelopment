using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public partial class GiteBed
    {
        public int GiteBedId { get; private set; }
        public int GiteId { get; private set; }
        public int Amount1PrBed { get; private set; }
        public int Amount2PrBed { get; private set; }
        public int Amount3PrBed { get; private set; }
        public string BedSort { get; private set; } = null!;

        [JsonIgnore]
        public Gite Gite { get; private set; } = null!;
    }
}
