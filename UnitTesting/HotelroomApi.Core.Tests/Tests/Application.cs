using Application.Abstractions.Persistence;
using Application.Hotelrooms.Queries;
using Domain.Models;

namespace HotelroomApi.Core.Tests.Application;

public sealed class HotelroomApplicationTests
{
    [Fact] //UT-HOTELROOM-APP-GETALL-001
    public async Task GetAllHotelroomsQuery_calls_repository_and_returns_rooms()
    {
        var rooms = new[]
        {
            Hotelroom.Rehydrate(1, 101, 50m, true, 1, 2),
            Hotelroom.Rehydrate(2, 102, 60m, false, 1, 3),
        };
        var repo = new FakeHotelroomRepository(rooms);
        var query = new GetAllHotelroomsQuery(repo);

        var result = await query.ExecuteAsync();

        Assert.Equal(1, repo.GetAllCalls);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].RoomId);
        Assert.Equal(2, result[1].RoomId);
    }

    [Fact] //UT-HOTELROOM-APP-GETBYID-002
    public async Task GetHotelroomByIdQuery_calls_repository_with_id_and_returns_room()
    {
        var rooms = new[]
        {
            Hotelroom.Rehydrate(10, 201, 70m, true, 1, 2),
        };
        var repo = new FakeHotelroomRepository(rooms);
        var query = new GetHotelroomByIdQuery(repo);

        var result = await query.ExecuteAsync(10);

        Assert.Equal(1, repo.GetByIdCalls);
        Assert.Equal(10, repo.LastId);
        Assert.NotNull(result);
        Assert.Equal(201, result!.RoomNumber);
    }

    [Fact] //UT-HOTELROOM-APP-GETBYID-003
    public async Task GetHotelroomByIdQuery_returns_null_when_not_found()
    {
        var repo = new FakeHotelroomRepository(Array.Empty<Hotelroom>());
        var query = new GetHotelroomByIdQuery(repo);

        var result = await query.ExecuteAsync(999);

        Assert.Null(result);
    }
}