using Application.Abstractions;
using Infrastructure.Persistence;

namespace Infrastructure.Persistence
{
    public sealed class EfUnitOfWork : IUnitOfWork
    {
        private readonly GiteDbContext _db;

        public EfUnitOfWork(GiteDbContext db)
        {
            _db = db;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
