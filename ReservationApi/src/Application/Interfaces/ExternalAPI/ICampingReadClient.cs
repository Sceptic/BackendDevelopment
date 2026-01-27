namespace Application.Abstractions;

public interface ICampingReadClient
{
    Task<CampingSnapshot> GetInfoAsync(CampingRequest request, CancellationToken ct);
}

public sealed record CampingRequest(int CampingId);

public sealed record CampingSnapshot(
    int CampingId);
