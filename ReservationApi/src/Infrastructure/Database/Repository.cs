using Domain.Models;
using Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class ReservationRepository : IReservationRepository
{
    private readonly ReservationDbContext _db;

    public ReservationRepository(ReservationDbContext db)
    {
        _db = db;
    }

    public async Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _db.Reservations.AddAsync(reservation, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return reservation;
    }

    public async Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await BaseReservationQuery()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Reservation?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return await BaseReservationQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ReservationId == reservationId, cancellationToken);
    }

    public async Task<Reservation?> GetByIdTrackedAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return await BaseReservationQuery()
            .FirstOrDefaultAsync(x => x.ReservationId == reservationId, cancellationToken);
    }

    public async Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _db.Reservations.Update(reservation);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Reservations
            .FirstOrDefaultAsync(x => x.ReservationId == reservationId, cancellationToken);

        if (existing is null)
            return;

        _db.Reservations.Remove(existing);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Reservation> BaseReservationQuery()
    {
        return _db.Reservations
            .Include(x => x.Clients)
            .Include(x => x.Gites)
            .Include(x => x.Hotelrooms)
            .Include(x => x.Campings)
            .Include(x => x.Facilities)
            .Include(x => x.Vehicles)
            .Include(x => x.Restaurants);
    }
}
