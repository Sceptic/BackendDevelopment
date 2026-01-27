namespace Application.Abstractions.Persistence;

public interface IReservationConflictQueries
{
    Task<bool> AnyRoomOverlapAsync(int roomId, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default);
    Task<bool> AnyGiteOverlapAsync(int giteId, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default);
    Task<bool> AnyCampingOverlapAsync(int campingId, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default);
    Task<bool> AnyFacilityOverlapAsync(string facility, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default);
    Task<bool> AnyVehicleOverlapAsync(string registrationPlate, DateTime start, DateTime end, int? excludeReservationId, CancellationToken ct = default);

    Task<bool> AnyTableOverlapAsync(
        int tableId,
        DateTime tableStart,
        DateTime tableEnd,
        int? excludeReservationId,
        int? excludeReservationRestaurantId,
        CancellationToken ct = default);
}
