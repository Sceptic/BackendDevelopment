namespace Application.Abstractions;

public interface IRestaurantReadClient
{
    Task<RestaurantSnapshot> GetInfoAsync(RestaurantRequest request, CancellationToken ct);
}

public sealed record RestaurantRequest(int TableId);

public sealed record RestaurantSnapshot(
    int TableId,
    int ?Capacity);

