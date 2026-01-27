using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Domain.Models;

namespace Application.Reservations;

public sealed partial class ReservationReadService : IReservationReadService
{
    private readonly IReservationRepository _repo;

    public ReservationReadService(IReservationRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<ReservationSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var reservations = await _repo.GetAllAsync(cancellationToken);
        return reservations.Select(ToSummaryDto).ToList();
    }

    public async Task<ReservationDto?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await _repo.GetByIdAsync(reservationId, cancellationToken);
        return reservation is null ? null : ToDto(reservation);
    }

    private static ReservationSummaryDto ToSummaryDto(Reservation r) =>
        new(
            r.ReservationId,
            r.AccountId,
            r.ReservationStatus,
            r.PaymentStatus,
            r.ReservationPrice,
            r.Discount,
            r.TouristTarif,
            r.ReservationStart,
            r.ReservationEnd
        );

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
            r.Clients.Select(c => new ReservationClientDto(c.ReservationId, c.FirstName, c.LastName, c.BirthDate)).ToList(),
            r.Gites.Select(g => new ReservationGiteDto(g.ReservationId, g.GiteId, g.GiteDiscount)).ToList(),
            r.Hotelrooms.Select(h => new ReservationHotelroomDto(h.ReservationId, h.RoomId, h.HotelroomDiscount)).ToList(),
            r.Campings.Select(c => new ReservationCampingDto(c.ReservationId, c.CampingId, c.CampingDiscount)).ToList(),
            r.Facilities.Select(f => new ReservationFacilityDto(f.ReservationId, f.Facility, f.FacilityDiscount)).ToList(),
            r.Vehicles.Select(v => new VehicleDto(v.ReservationId, v.RegistrationPlate)).ToList(),
            r.Restaurants.Select(rr => new ReservationRestaurantDto(
                rr.ReservationRestaurantId,
                rr.ReservationId,
                rr.TableId,
                rr.TableReservationStart,
                rr.TableReservationEnd,
                rr.TableBill,
                rr.TableDiscount
            )).ToList()
        );
}
