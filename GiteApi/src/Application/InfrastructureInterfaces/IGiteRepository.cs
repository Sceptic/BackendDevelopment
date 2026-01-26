using Domain.Models;

namespace Application.Abstractions
{
    public interface IGiteRepository
    {
        // Read
        Task<Gite?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Gite>> GetAllAsync(CancellationToken ct = default);

        // Write
        Task AddAsync(Gite gite, CancellationToken ct = default);
        Task UpdateAsync(Gite gite, CancellationToken ct = default);
        Task DeleteAsync(Gite gite, CancellationToken ct = default);
    }
}
