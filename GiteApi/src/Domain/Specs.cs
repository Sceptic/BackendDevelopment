using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Specs
{
    public sealed record GiteAmenitiesSpec
    (
        bool? Wifi, bool? Bath, bool? Shower, bool? HairDryer, bool? SmallChild,
        bool? Toiletries, bool? Desk, bool? Chair, bool? Balcony, bool? Sofa,
        bool? SofaBed, bool? MiniFridge, bool? Kettle, bool? Cuttlery,
        bool? EatingArea, bool? RoomService
    );

    public sealed record GiteBedSpec
    (
        int Amount1PrBed, int Amount2PrBed, int Amount3PrBed, string BedSort
    );
}

