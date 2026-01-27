using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Domain.Models;

namespace Application.Reservations;

public sealed partial class ReservationCommandService : IReservationCommandService
{
    //Database Registration
    private readonly IReservationRepository _repo;

    //Cross-Aggregate Checker, also invokes the DB
    private readonly IReservationAvailabilityChecker _availability;

    //External APIs
    private readonly IReservationExternalPolicy _externalPolicy;

    public ReservationCommandService(
        IReservationRepository repo,
        IReservationAvailabilityChecker availability,

        IReservationExternalPolicy externalPolicy
        )
    {
        _repo = repo;
        _availability = availability;

        _externalPolicy = externalPolicy;
    }

    private static ReservationDto ToDto(Reservation r) =>
        new(
            r.ReservationId,
            r.AccountId,
            r.ReservationStatus,
            r.PaymentStatus,
            r.ReservationPrice,
            r.Discount,
            r.TouristTarif,
            r.ReservationStart,
            r.ReservationEnd,
            r.Clients.Select(c => new ReservationClientDto(r.ReservationId, c.FirstName, c.LastName, c.BirthDate)).ToList(),
            r.Gites.Select(g => new ReservationGiteDto(r.ReservationId, g.GiteId, g.GiteDiscount)).ToList(),
            r.Hotelrooms.Select(h => new ReservationHotelroomDto(r.ReservationId, h.RoomId, h.HotelroomDiscount)).ToList(),
            r.Campings.Select(c => new ReservationCampingDto(r.ReservationId, c.CampingId, c.CampingDiscount)).ToList(),
            r.Facilities.Select(f => new ReservationFacilityDto(r.ReservationId, f.Facility, f.FacilityDiscount)).ToList(),
            r.Vehicles.Select(v => new VehicleDto(r.ReservationId, v.RegistrationPlate)).ToList(),
            r.Restaurants.Select(rr => new ReservationRestaurantDto(
                rr.ReservationRestaurantId,
                r.ReservationId,
                rr.TableId,
                rr.TableReservationStart,
                rr.TableReservationEnd,
                rr.TableBill,
                rr.TableDiscount
            )).ToList()
        );
}
