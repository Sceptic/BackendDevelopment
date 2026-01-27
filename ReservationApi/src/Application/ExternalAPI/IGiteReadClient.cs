namespace Application.Abstractions;

public interface IGiteReadClient
{
    Task<GiteSnapshot> GetInfoAsync(GiteRequest request, CancellationToken ct);
}

public sealed record GiteRequest(int GiteId);

public sealed record GiteSnapshot(
    int GiteId,
    decimal GitePrice,
    bool IsAvailable,
    int CapacityMin,
    int CapacityMax);

