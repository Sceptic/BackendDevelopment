using Domain.ErrorHandling;
using Domain.Models;

namespace ReservationApi.Core.Tests;

public sealed class ReservationDomainTests
{
    private static Reservation MakeValidReservation()
    {
        var start = new DateTime(2026, 06, 01, 14, 00, 00);
        var end = new DateTime(2026, 06, 10, 10, 00, 00);

        var r = new Reservation
        {
            AccountId = 1001,
            ReservationStatus = "CONFIRMED",
            PaymentStatus = "PAID",
            ReservationPrice = 1200m,
            Discount = 0.10m,
            TouristTarif = 0.05m,
            ReservationStart = start,
            ReservationEnd = end,
        };

        r.Clients.Add(new ReservationClient { FirstName = "Anna", LastName = "Peeters", BirthDate = new DateTime(1985, 03, 22) });
        r.Gites.Add(new ReservationGite { GiteId = 501, GiteDiscount = 0.15m });
        r.Hotelrooms.Add(new ReservationHotelroom { RoomId = 301, HotelroomDiscount = 0.05m });
        r.Campings.Add(new ReservationCamping { CampingId = 1, CampingDiscount = 0.20m });
        r.Facilities.Add(new ReservationFacility { Facility = "Sauna", FacilityDiscount = 0.15m });
        r.Vehicles.Add(new Vehicle { RegistrationPlate = "1-ABC-123" });

        r.Restaurants.Add(new ReservationRestaurant
        {
            TableId = 10,
            TableReservationStart = start.AddHours(4),
            TableReservationEnd = start.AddHours(6),
            TableBill = 140m,
            TableDiscount = 0.10m
        });

        return r;
    }

    [Fact] //UT-RES-DOMAIN-VALID-001
    public void EnsureValid_accepts_well_formed_aggregate()
    {
        var r = MakeValidReservation();

        r.EnsureValid();
    }

    [Fact] //UT-RES-DOMAIN-PRICING-002
    public void EnsureValid_rejects_discount_outside_0_to_1()
    {
        var r = MakeValidReservation();
        r.Discount = 1.10m;

        var ex = Assert.Throws<DomainValidationException>(() => r.EnsureValid());
        Assert.Contains("Discount", ex.Message);
    }

    [Fact] //UT-RES-DOMAIN-DATE-003
    public void SetPeriod_rejects_end_not_after_start()
    {
        var r = MakeValidReservation();
        var start = new DateTime(2026, 06, 01, 14, 00, 00);
        var end = start;

        var ex = Assert.Throws<DomainValidationException>(() => r.SetPeriod(start, end));
        Assert.Contains("after", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact] //UT-RES-DOMAIN-RESTAURANT-004
    public void Restaurant_must_fall_within_reservation_period()
    {
        var r = MakeValidReservation();
        r.Restaurants.Clear();

        r.Restaurants.Add(new ReservationRestaurant
        {
            TableId = 10,
            TableReservationStart = r.ReservationEnd.AddHours(1),
            TableReservationEnd = r.ReservationEnd.AddHours(2),
            TableBill = 50m,
            TableDiscount = 0.0m
        });

        var ex = Assert.Throws<DomainValidationException>(() => r.EnsureValid());
        Assert.Contains("within the reservation period", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
