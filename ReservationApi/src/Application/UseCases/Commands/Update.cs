using Application.DTOs.Reservations;
using Domain.Models;

namespace Application.Reservations;

public sealed partial class ReservationCommandService
{
    public async Task<ReservationDto?> PatchAsync(PatchReservationRequestDto request, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdTrackedAsync(request.ReservationId, cancellationToken);
        if (existing is null) return null;

        // Scalars
        if (request.AccountId.HasValue)
            existing.AccountId = request.AccountId.Value;

        // Statuses: apply only if provided (individually)
        if (request.ReservationStatus is not null)
            existing.ReservationStatus = request.ReservationStatus;

        if (request.PaymentStatus is not null)
            existing.PaymentStatus = request.PaymentStatus;

        // Pricing: apply only what is provided
        if (request.ReservationPrice.HasValue)
            existing.ReservationPrice = request.ReservationPrice.Value;

        if (request.Discount.HasValue)
            existing.Discount = request.Discount.Value;

        if (request.TouristTarif.HasValue)
            existing.TouristTarif = request.TouristTarif.Value;

        // Period: need both values to safely call SetPeriod.
        // If only one is provided, merge with existing.
        if (request.ReservationStart.HasValue || request.ReservationEnd.HasValue)
        {
            var start = request.ReservationStart ?? existing.ReservationStart;
            var end = request.ReservationEnd ?? existing.ReservationEnd;
            existing.SetPeriod(start, end);
        }

        // Collections: only replace if provided (non-null)
        if (request.Clients is not null)
        {
            existing.Clients = request.Clients.Select(c => new ReservationClient
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                BirthDate = c.BirthDate
            }).ToList();
        }

        if (request.Gites is not null)
        {
            existing.Gites = request.Gites.Select(g => new ReservationGite
            {
                GiteId = g.GiteId,
                GiteDiscount = g.GiteDiscount
            }).ToList();
        }

        if (request.Hotelrooms is not null)
        {
            existing.Hotelrooms = request.Hotelrooms.Select(h => new ReservationHotelroom
            {
                RoomId = h.RoomId,
                HotelroomDiscount = h.HotelroomDiscount
            }).ToList();
        }

        if (request.Campings is not null)
        {
            existing.Campings = request.Campings.Select(c => new ReservationCamping
            {
                CampingId = c.CampingId,
                CampingDiscount = c.CampingDiscount
            }).ToList();
        }

        if (request.Facilities is not null)
        {
            existing.Facilities = request.Facilities.Select(f => new ReservationFacility
            {
                Facility = f.Facility,
                FacilityDiscount = f.FacilityDiscount
            }).ToList();
        }

        if (request.Vehicles is not null)
        {
            existing.Vehicles = request.Vehicles.Select(v => new Vehicle
            {
                RegistrationPlate = v.RegistrationPlate
            }).ToList();
        }

        if (request.Restaurants is not null)
        {
            existing.Restaurants = request.Restaurants.Select(r => new ReservationRestaurant
            {
                ReservationRestaurantId = r.ReservationRestaurantId,
                TableId = r.TableId,
                TableReservationStart = r.TableReservationStart,
                TableReservationEnd = r.TableReservationEnd,
                TableBill = r.TableBill,
                TableDiscount = r.TableDiscount
            }).ToList();
        }

        existing.EnsureValid();

        await _availability.EnsureNoConflictsAsync(existing, excludeReservationId: existing.ReservationId, cancellationToken);

        existing.EnsureValid();
        await _externalPolicy.ApplyAsync(existing, cancellationToken);
        existing.EnsureValid();
        await _repo.UpdateAsync(existing, cancellationToken);

        var updated = await _repo.GetByIdAsync(existing.ReservationId, cancellationToken);
        return updated is null ? null : ToDto(updated);
    }
}
