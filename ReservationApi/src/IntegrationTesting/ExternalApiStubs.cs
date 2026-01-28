using Application.Abstractions;
using Application.Abstractions.Reservations;

namespace IntegrationTesting.Helpers;
public sealed class StubGiteReadClient : IGiteReadClient
{
    public Task<GiteSnapshot> GetInfoAsync(GiteRequest request, CancellationToken ct)
    {
        var snapshot = new GiteSnapshot(
            GiteId: request.GiteId,
            GitePrice: 100m,
            IsAvailable: true,
            CapacityMin: 1,
            CapacityMax: 4);

        return Task.FromResult(snapshot);
    }
}

public sealed class StubHotelroomReadClient : IHotelroomReadClient
{
    public Task<HotelroomSnapshot> GetInfoAsync(HotelroomRequest request, CancellationToken ct)
    {
        var snapshot = new HotelroomSnapshot(
            RoomId: request.RoomId,
            HotelroomPrice: 120m,
            IsAvailable: true,
            CapacityMin: 1,
            CapacityMax: 2);

        return Task.FromResult(snapshot);
    }
}

public sealed class StubCampingReadClient : ICampingReadClient
{
    public Task<CampingSnapshot> GetInfoAsync(CampingRequest request, CancellationToken ct)
        => Task.FromResult(new CampingSnapshot(request.CampingId));
}

public sealed class StubRestaurantReadClient : IRestaurantReadClient
{
    public Task<RestaurantSnapshot> GetInfoAsync(RestaurantRequest request, CancellationToken ct)
    {
        var snapshot = new RestaurantSnapshot(
            TableId: request.TableId,
            Capacity: 4);

        return Task.FromResult(snapshot);
    }
}
