using Application.Reservations;
using Domain.Models;

namespace ReservationApi.Core.Tests.Application;

public sealed partial class ReservationApplicationTests
{
    [Fact] //UT-RES-APP-READ-001
    public async Task ReadService_delegates_to_repository_and_maps_dtos()
    {
        var repo = new FakeReservationRepository();

        var reservation = MakeReservation(accountId: 1001);
        reservation.Clients.Add(new ReservationClient { FirstName = "Anna", LastName = "Peeters", BirthDate = new DateTime(1985, 03, 22) });
        reservation.Gites.Add(new ReservationGite { GiteId = 501, GiteDiscount = 0.15m });
        reservation.Hotelrooms.Add(new ReservationHotelroom { RoomId = 301, HotelroomDiscount = 0.05m });
        reservation.Campings.Add(new ReservationCamping { CampingId = 1, CampingDiscount = 0.20m });
        reservation.Facilities.Add(new ReservationFacility { Facility = "Sauna", FacilityDiscount = 0.15m });
        reservation.Vehicles.Add(new Vehicle { RegistrationPlate = "1-ABC-123" });
        reservation.Restaurants.Add(new ReservationRestaurant
        {
            TableId = 10,
            TableReservationStart = reservation.ReservationStart.AddHours(4),
            TableReservationEnd = reservation.ReservationStart.AddHours(6),
            TableBill = 140m,
            TableDiscount = 0.10m
        });

        repo.Seed(7, reservation);

        var service = new ReservationReadService(repo);

        var found = await service.GetByIdAsync(7, CancellationToken.None);
        Assert.Equal(1, repo.GetByIdCalls);
        Assert.NotNull(found);
        Assert.Equal(7, found!.ReservationId);
        Assert.Single(found.Clients);
        Assert.Single(found.Gites);
        Assert.Single(found.Restaurants);

        var all = await service.GetAllAsync(CancellationToken.None);
        Assert.Equal(1, repo.GetAllCalls);
        Assert.Single(all);
        Assert.Equal(7, all[0].ReservationId);
        Assert.Equal(1001, all[0].AccountId);
    }
}
