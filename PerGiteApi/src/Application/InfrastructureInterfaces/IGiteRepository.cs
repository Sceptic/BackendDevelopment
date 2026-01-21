using Domain.Models;

namespace Application.Abstractions
{
    public interface IGiteRepository
    {
        //Read methods
        Task<Gite?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Gite>> GetAllAsync(CancellationToken ct = default);

        //Write Methods
        Task AddAsync(Gite gite, CancellationToken ct = default);
        void Update(Gite gite);
        void Delete(Gite gite);
    }
}


