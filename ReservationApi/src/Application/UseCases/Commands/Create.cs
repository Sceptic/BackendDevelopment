using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Domain.Models;

namespace Application.Reservations;

public sealed partial class ReservationCommandService : IReservationCommandService
{
    public async Task<ReservationDto> CreateAsync(CreateReservationRequestDto request, CancellationToken cancellationToken = default)
    {
        var reservation = new Reservation
        {
            AccountId = request.AccountId,
            ReservationStatus = request.ReservationStatus,
            PaymentStatus = request.PaymentStatus,
            ReservationPrice = request.ReservationPrice,
            Discount = request.Discount,
            TouristTarif = request.TouristTarif,
            ReservationStart = request.ReservationStart,
            ReservationEnd = request.ReservationEnd,
            Clients = request.Clients.Select(c => new ReservationClient
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                BirthDate = c.BirthDate
            }).ToList(),
            Gites = request.Gites.Select(g => new ReservationGite
            {
                GiteId = g.GiteId,
                GiteDiscount = g.GiteDiscount
            }).ToList(),
            Hotelrooms = request.Hotelrooms.Select(h => new ReservationHotelroom
            {
                RoomId = h.RoomId,
                HotelroomDiscount = h.HotelroomDiscount
            }).ToList(),
            Campings = request.Campings.Select(c => new ReservationCamping
            {
                CampingId = c.CampingId,
                CampingDiscount = c.CampingDiscount
            }).ToList(),
            Facilities = request.Facilities.Select(f => new ReservationFacility
            {
                Facility = f.Facility,
                FacilityDiscount = f.FacilityDiscount
            }).ToList(),
            Vehicles = request.Vehicles.Select(v => new Vehicle
            {
                RegistrationPlate = v.RegistrationPlate
            }).ToList(),
            Restaurants = request.Restaurants.Select(r => new ReservationRestaurant
            {
                TableId = r.TableId,
                TableReservationStart = r.TableReservationStart,
                TableReservationEnd = r.TableReservationEnd,
                TableBill = r.TableBill,
                TableDiscount = r.TableDiscount
            }).ToList(),
        };

        await _availability.EnsureNoConflictsAsync(reservation, excludeReservationId: null, cancellationToken);

        reservation.EnsureValid();
        await _externalPolicy.ApplyAsync(reservation, cancellationToken);
        reservation.EnsureValid();
        var created = await _repo.CreateAsync(reservation, cancellationToken);

        return ToDto(created);
    }
}
