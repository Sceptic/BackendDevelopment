namespace Application.DtoModels
{
    public sealed record GiteAmenitiesDto
    {
        public bool? Wifi { get; init; }
        public bool? Bath { get; init; }
        public bool? Shower { get; init; }
        public bool? HairDryer { get; init; }
        public bool? SmallChild { get; init; }
        public bool? Toiletries { get; init; }
        public bool? Desk { get; init; }
        public bool? Chair { get; init; }
        public bool? Balcony { get; init; }
        public bool? Sofa { get; init; }
        public bool? SofaBed { get; init; }
        public bool? MiniFridge { get; init; }
        public bool? Kettle { get; init; }
        public bool? Cuttlery { get; init; }
        public bool? EatingArea { get; init; }
        public bool? RoomService { get; init; }
    }
}
