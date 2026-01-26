using Application.Abstractions;
using Domain.Models;

namespace Application.Gites.ReadQueries
{
    public sealed class GiteReadingService
    {
        private readonly IGiteRepository _repo;

        public GiteReadingService(IGiteRepository repo)
        {
            _repo = repo;
        }

        public async Task<Gite?> GetByIdAsync(int giteId, CancellationToken ct)
        {
            return await _repo.GetByIdAsync(giteId, ct);

        }

        public async Task<IReadOnlyList<Gite>> GetAllAsync(CancellationToken ct)
        {
            return await _repo.GetAllAsync(ct);
        }
    }
}
