using Application.Abstractions.Persistence;
using Domain.Models;
using System.Reflection;

namespace ReservationApi.Core.Tests.Application;

internal sealed class FakeReservationRepository : IReservationRepository
{
    private static void SetReservationId(Reservation reservation, int id)
    {
        reservation.ReservationId = id;

        foreach (var c in reservation.Clients) c.ReservationId = id;
        foreach (var g in reservation.Gites) g.ReservationId = id;
        foreach (var h in reservation.Hotelrooms) h.ReservationId = id;
        foreach (var c in reservation.Campings) c.ReservationId = id;
        foreach (var f in reservation.Facilities) f.ReservationId = id;
        foreach (var v in reservation.Vehicles) v.ReservationId = id;
        foreach (var r in reservation.Restaurants) r.ReservationId = id;
    }

    private readonly Dictionary<int, Reservation> _store = new();
    private int _nextId = 1;
    private int _nextRestaurantId = 1;

    public int CreateCalls { get; private set; }
    public int GetByIdCalls { get; private set; }
    public int GetByIdTrackedCalls { get; private set; }
    public int GetAllCalls { get; private set; }
    public int UpdateCalls { get; private set; }
    public int DeleteCalls { get; private set; }

    public Reservation? LastCreated { get; private set; }
    public Reservation? LastTracked { get; private set; }
    public Reservation? LastUpdated { get; private set; }
    public int? LastDeletedId { get; private set; }

    public void Seed(int id, Reservation reservation)
    {
        if (id >= _nextId) _nextId = id + 1;
        SetReservationId(reservation, id);

        foreach (var rr in reservation.Restaurants)
        {
            if (rr.ReservationRestaurantId <= 0)
                rr.ReservationRestaurantId = _nextRestaurantId++;
        }

        _store[id] = reservation;
    }

    public Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        CreateCalls++;

        var id = _nextId++;
        SetReservationId(reservation, id);

        foreach (var rr in reservation.Restaurants)
        {
            if (rr.ReservationRestaurantId <= 0)
                rr.ReservationRestaurantId = _nextRestaurantId++;
        }

        _store[id] = reservation;
        LastCreated = reservation;
        return Task.FromResult(reservation);
    }

    public Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        GetAllCalls++;
        return Task.FromResult((IReadOnlyList<Reservation>)_store.Values.OrderBy(r => r.ReservationId).ToList());
    }

    public Task<Reservation?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        GetByIdCalls++;
        _store.TryGetValue(reservationId, out var r);
        return Task.FromResult(r);
    }

    public Task<Reservation?> GetByIdTrackedAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        GetByIdTrackedCalls++;
        _store.TryGetValue(reservationId, out var r);
        LastTracked = r;
        return Task.FromResult(r);
    }

    public Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        UpdateCalls++;
        LastUpdated = reservation;
        _store[reservation.ReservationId] = reservation;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        DeleteCalls++;
        LastDeletedId = reservationId;
        _store.Remove(reservationId);
        return Task.CompletedTask;
    }
}
