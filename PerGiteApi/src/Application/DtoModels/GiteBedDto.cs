namespace Api.DtoModels
{
    public sealed record GiteBedDto
    {
        public int Amount1PrBed { get; init; }
        public int Amount2PrBed { get; init; }
        public int Amount3PrBed { get; init; }
        public string BedSort { get; init; } = null!;
    }
}
