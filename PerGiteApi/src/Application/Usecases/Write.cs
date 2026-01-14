using Application.Abstractions;
using Domain.Models;

namespace Application.Gites.WriteQueries
{
    public sealed class GiteWritingService
    {
        private readonly IGiteRepository _repo;
        private readonly IUnitOfWork _uow;

        public GiteWritingService(IGiteRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task CreateAsync(Gite gite, CancellationToken ct)
        {
            await _repo.AddAsync(gite, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Gite gite, CancellationToken ct)
        {
            await _repo.UpdateAsync(gite, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var gite = await _repo.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException("Gite not found");

            await _repo.DeleteAsync(gite, ct);
            await _uow.SaveChangesAsync(ct);
        }
    }
}

