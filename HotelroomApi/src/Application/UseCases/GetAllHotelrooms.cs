using Application.Abstractions.Persistence;
using Domain.Models;

namespace Application.Hotelrooms.Queries;

public sealed class GetAllHotelroomsQuery
{
    private readonly IHotelroomRepository _repository;

    public GetAllHotelroomsQuery(IHotelroomRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<Hotelroom>> ExecuteAsync()
        => _repository.GetAllAsync();
}
