using Application.Abstractions.Persistence;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelroomApi.Core.Tests.Application;

internal sealed class FakeHotelroomRepository : IHotelroomRepository
{
    public int GetAllCalls { get; private set; }
    public int GetByIdCalls { get; private set; }
    public int? LastId { get; private set; }

    private readonly IReadOnlyList<Hotelroom> _all;
    private readonly Dictionary<int, Hotelroom> _byId;

    public FakeHotelroomRepository(IEnumerable<Hotelroom> rooms)
    {
        var list = rooms.ToList();
        _all = list;
        _byId = list.ToDictionary(r => r.RoomId);
    }

    public Task<IReadOnlyList<Hotelroom>> GetAllAsync()
    {
        GetAllCalls++;
        return Task.FromResult(_all);
    }

    public Task<Hotelroom?> GetByIdAsync(int roomId)
    {
        GetByIdCalls++;
        LastId = roomId;
        _byId.TryGetValue(roomId, out var room);
        return Task.FromResult(room);
    }
}
