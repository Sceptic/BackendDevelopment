namespace Application.DtoModels
{
    public sealed record GiteDto
    {
        public int GiteNumber { get; init; }
        public decimal GitePrice { get; init; }
        public bool IsAvailable { get; init; }
        public string GiteAddress { get; init; } = null!;
        public int CapacityMin { get; init; }
        public int CapacityMax { get; init; }

        public GiteAmenitiesDto Amenities { get; init; } = null!;
        public List<GiteBedDto> Beds { get; init; } = new();
    }
}
