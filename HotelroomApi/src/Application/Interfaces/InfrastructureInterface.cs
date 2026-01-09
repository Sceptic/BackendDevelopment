using Domain.Models;

namespace Application.Abstractions.Persistence;

public interface IHotelroomRepository
{
    Task<IReadOnlyList<Hotelroom>> GetAllAsync();
    Task<Hotelroom?> GetByIdAsync(int roomId);
}
