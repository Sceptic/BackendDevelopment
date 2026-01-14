using Application.Abstractions;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class GiteRepository : IGiteRepository
{
    private readonly GiteDbContext _db;

    public GiteRepository(GiteDbContext db)
    {
        _db = db;
    }

    public async Task<Gite?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Gites
            .Include(x => x.Amenities)
            .Include(x => x.Beds)
            .FirstOrDefaultAsync(x => x.GiteId == id, ct);
    }

    public async Task<IReadOnlyList<Gite>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Gites
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Gite gite, CancellationToken ct = default)
    {
        await _db.Gites.AddAsync(gite, ct);
    }

    public Task UpdateAsync(Gite gite, CancellationToken ct = default)
    {
        _db.Gites.Update(gite);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Gite gite, CancellationToken ct = default)
    {
        _db.Gites.Remove(gite);
        return Task.CompletedTask;
    }
}
