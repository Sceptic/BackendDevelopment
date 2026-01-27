namespace Application.DTOs.Reservations;

public sealed record PatchReservationRequestDto
(
    int ReservationId,

    int? AccountId,
    string? ReservationStatus,
    string? PaymentStatus,

    decimal? ReservationPrice,
    decimal? Discount,
    decimal? TouristTarif,

    DateTime? ReservationStart,
    DateTime? ReservationEnd,

    IReadOnlyList<PatchReservationClientDto>? Clients,
    IReadOnlyList<PatchReservationGiteDto>? Gites,
    IReadOnlyList<PatchReservationHotelroomDto>? Hotelrooms,
    IReadOnlyList<PatchReservationCampingDto>? Campings,
    IReadOnlyList<PatchReservationFacilityDto>? Facilities,
    IReadOnlyList<PatchVehicleDto>? Vehicles,
    IReadOnlyList<PatchReservationRestaurantDto>? Restaurants
);

public sealed record PatchReservationClientDto(string FirstName, string LastName, DateTime BirthDate);
public sealed record PatchReservationGiteDto(int GiteId, decimal GiteDiscount);
public sealed record PatchReservationHotelroomDto(int RoomId, decimal HotelroomDiscount);
public sealed record PatchReservationCampingDto(int CampingId, decimal CampingDiscount);
public sealed record PatchReservationFacilityDto(string Facility, decimal FacilityDiscount);
public sealed record PatchVehicleDto(string RegistrationPlate);
public sealed record PatchReservationRestaurantDto(
    int ReservationRestaurantId, // keep as 0 when updating or creating, value is automatically determined by database at persistance
    int TableId,
    DateTime TableReservationStart,
    DateTime TableReservationEnd,
    decimal TableBill,
    decimal TableDiscount
);
