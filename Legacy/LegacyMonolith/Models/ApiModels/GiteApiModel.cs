namespace LegacyMonolith.Models
{
    public class GiteDto
    {
        public int GiteNumber { get; set; }
        public decimal? GitePrice { get; set; }
        public bool? IsAvailable { get; set; }
        public string? GiteAddress { get; set; }
        public int? CapacityMin { get; set; }
        public int? CapacityMax { get; set; }
    }
}
