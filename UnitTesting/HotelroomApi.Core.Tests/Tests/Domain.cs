using Application.Abstractions.Persistence;
using Application.Hotelrooms.Queries;
using Domain.Models;

namespace HotelroomApi.Core.Tests.Domain;

public sealed class HotelroomDomainTests
{
    [Fact] //UT-HOTELROOM-DOMAIN-REHYDRATE-001
    public void Rehydrate_sets_properties()
    {
        var room = Hotelroom.Rehydrate(
            roomId: 10,
            roomNumber: 101,
            hotelroomPrice: 99.95m,
            isAvailable: true,
            capacityMin: 1,
            capacityMax: 3);

        Assert.Equal(10, room.RoomId);
        Assert.Equal(101, room.RoomNumber);
        Assert.Equal(99.95m, room.HotelroomPrice);
        Assert.True(room.IsAvailable);
        Assert.Equal(1, room.CapacityMin);
        Assert.Equal(3, room.CapacityMax);
        Assert.NotNull(room.Beds);
        Assert.Empty(room.Beds);
        Assert.Null(room.Amenities);
    }

    [Fact] //UT-HOTELROOM-DOMAIN-AMENITIES-002
    public void AttachAmenities_sets_reference()
    {
        var room = Hotelroom.Rehydrate(1, 1, null, null, null, null);

        var amenities = HotelroomAmenities.Rehydrate(
            roomId: 1,
            room: room,
            wifi: true,
            bath: null,
            shower: true,
            hairdryer: null,
            smallchild: null,
            toiletries: null,
            desk: null,
            chair: null,
            balcony: null,
            sofa: null,
            sofabed: null,
            minifridge: null,
            kettle: null,
            cuttlery: null,
            eatingarea: null,
            roomservice: null);

        room.AttachAmenities(amenities);

        Assert.NotNull(room.Amenities);
        Assert.True(room.Amenities!.Wifi);
        Assert.True(room.Amenities!.Shower);
        Assert.Equal(1, room.Amenities!.RoomId);
        Assert.Same(room, room.Amenities!.Room);
    }

    [Fact] //UT-HOTELROOM-DOMAIN-BED-003
    public void AddBed_adds_to_list()
    {
        var room = Hotelroom.Rehydrate(1, 1, null, null, null, null);

        var bed = HotelroomBed.Rehydrate(
            hotelroomBedId: 5,
            roomId: 1,
            room: room,
            amount1PrBed: 1,
            amount2PrBed: 0,
            amount3PrBed: 0,
            bedSort: "single");

        room.AddBed(bed);

        Assert.Single(room.Beds);
        Assert.Same(bed, room.Beds[0]);
        Assert.Same(room, bed.Room);
    }

    [Fact] //UT-HOTELROOM-DOMAIN-BED-004
    public void AddBed_allows_multiple_beds()
    {
        var room = Hotelroom.Rehydrate(1, 1, null, null, null, null);

        var bed1 = HotelroomBed.Rehydrate(
            hotelroomBedId: 5,
            roomId: 1,
            room: room,
            amount1PrBed: 1,
            amount2PrBed: 0,
            amount3PrBed: 0,
            bedSort: "single");

        var bed2 = HotelroomBed.Rehydrate(
            hotelroomBedId: 6,
            roomId: 1,
            room: room,
            amount1PrBed: 0,
            amount2PrBed: 1,
            amount3PrBed: 0,
            bedSort: "double");

        room.AddBed(bed1);
        room.AddBed(bed2);

        Assert.Equal(2, room.Beds.Count);
        Assert.Same(bed1, room.Beds[0]);
        Assert.Same(bed2, room.Beds[1]);
        Assert.Same(room, bed1.Room);
        Assert.Same(room, bed2.Room);
    }
}