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

    public async Task<Hotelroom?> ExecuteAsync(int roomId)
    {
        var room = await _repository.GetByIdAsync(roomId);

        if (room is null)
            return null;

        if (room.IsAvailable == false)
            return null;

        return room;
    }
}
