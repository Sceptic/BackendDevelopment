using Application.Abstractions;
using Application.DtoModels;
using Application.Gites.ReadQueries;
using Application.Gites.WriteQueries;
using Domain.ErrorHandling;
using Domain.Models;
using Domain.Specs;

namespace GiteApi.Core.Tests;

public sealed class GiteDomainTests
{
    //Helper method, maakt een standaard lijst van voorzieningen zodat dat niet handmatig elke keer hoeft.
    private static GiteAmenitiesSpec DefaultAmenities() => new(
        Wifi: true, Bath: false, Shower: true, HairDryer: false, SmallChild: false,
        Toiletries: false, Desk: false, Chair: false, Balcony: false, Sofa: false,
        SofaBed: false, MiniFridge: false, Kettle: false, Cuttlery: false,
        EatingArea: false, RoomService: false);

    //Helper method, zelfde geldt hier, maar dan voor de bedden.
    private static IEnumerable<GiteBedSpec> DefaultBeds() => new[]
    {
        new GiteBedSpec(Amount1PrBed: 1, Amount2PrBed: 0, Amount3PrBed: 0, BedSort: "single")
    };

    [Fact] //UT-GITE-DOMAIN-CREATE-001
    public void Create_builds_gite_with_amenities_and_beds()
    {
        var gite = Gite.Create(
            giteNumber: 12,
            gitePrice: 123.45m,
            isAvailable: true,
            giteAddress: "Teststraat 1",
            capacityMin: 1,
            capacityMax: 4,
            amenities: DefaultAmenities(),
            beds: DefaultBeds());

        Assert.Equal(12, gite.GiteNumber);
        Assert.Equal(123.45m, gite.GitePrice);
        Assert.True(gite.IsAvailable);
        Assert.Equal("Teststraat 1", gite.GiteAddress);
        Assert.Equal(1, gite.CapacityMin);
        Assert.Equal(4, gite.CapacityMax);

        Assert.NotNull(gite.Amenities);
        Assert.True(gite.Amenities!.Wifi);
        Assert.True(gite.Amenities!.Shower);

        Assert.NotNull(gite.Beds);
        Assert.Single(gite.Beds);
    }

    [Fact] //UT-GITE-DOMAIN-PRICE-002
    public void Create_rejects_negative_price()
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Gite.Create(1, -1m, true, "A", 1, 1, DefaultAmenities(), DefaultBeds()));

        Assert.Contains("GitePrice", ex.Message);
    }

    [Fact] //UT-GITE-DOMAIN-BED-003
    public void RemoveBed_rejects_removing_last_bed()
    {
        var gite = Gite.Create(1, 10m, true, "A", 1, 2, DefaultAmenities(), DefaultBeds());
        var onlyBed = gite.Beds.Single();

        var ex = Assert.Throws<DomainValidationException>(() => gite.RemoveBed(onlyBed));
        Assert.Contains("at least one bed", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact] //UT-GITE-DOMAIN-BED-004
    public void ReplaceBeds_replaces_collection()
    {
        var gite = Gite.Create(1, 10m, true, "A", 1, 2, DefaultAmenities(), DefaultBeds());

        gite.ReplaceBeds(new[]
        {
            new GiteBedSpec(1, 1, 0, "double"),
            new GiteBedSpec(0, 1, 0, "sofa"),
        });

        Assert.Equal(2, gite.Beds.Count);
        Assert.Contains(gite.Beds, b => b.BedSort == "double");
        Assert.Contains(gite.Beds, b => b.BedSort == "sofa");
    }
}