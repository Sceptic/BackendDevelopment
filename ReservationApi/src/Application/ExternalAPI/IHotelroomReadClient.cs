namespace Application.Abstractions;

public interface IHotelroomReadClient
{
    Task<HotelroomSnapshot> GetInfoAsync(HotelroomRequest request, CancellationToken ct);
}

public sealed record HotelroomRequest(int RoomId);

public sealed record HotelroomSnapshot(
    int RoomId,
    decimal HotelroomPrice,
    bool IsAvailable,
    int CapacityMin,
    int CapacityMax);

