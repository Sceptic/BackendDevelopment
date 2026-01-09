using Application.Abstractions.Persistence;
using Domain.Models;

namespace Application.Hotelrooms.Queries;

public sealed class GetHotelroomByIdQuery
{
    private readonly IHotelroomRepository _repository;

    public GetHotelroomByIdQuery(IHotelroomRepository repository)
    {
        _repository = repository;
    }

    public Task<Hotelroom?> ExecuteAsync(int roomId)
        => _repository.GetByIdAsync(roomId);
}
