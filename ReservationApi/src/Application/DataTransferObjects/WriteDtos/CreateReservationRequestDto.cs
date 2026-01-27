namespace Application.DTOs.Reservations;

public sealed record CreateReservationRequestDto
(
    int AccountId,
    string ReservationStatus,
    string PaymentStatus,
    decimal ReservationPrice,
    decimal Discount,
    decimal TouristTarif,
    DateTime ReservationStart,
    DateTime ReservationEnd,
    IReadOnlyList<CreateReservationClientDto> Clients,
    IReadOnlyList<CreateReservationGiteDto> Gites,
    IReadOnlyList<CreateReservationHotelroomDto> Hotelrooms,
    IReadOnlyList<CreateReservationCampingDto> Campings,
    IReadOnlyList<CreateReservationFacilityDto> Facilities,
    IReadOnlyList<CreateVehicleDto> Vehicles,
    IReadOnlyList<CreateReservationRestaurantDto> Restaurants
);

public sealed record CreateReservationClientDto(string FirstName, string LastName, DateTime BirthDate);
public sealed record CreateReservationGiteDto(int GiteId, decimal GiteDiscount);
public sealed record CreateReservationHotelroomDto(int RoomId, decimal HotelroomDiscount);
public sealed record CreateReservationCampingDto(int CampingId, decimal CampingDiscount);
public sealed record CreateReservationFacilityDto(string Facility, decimal FacilityDiscount);
public sealed record CreateVehicleDto(string RegistrationPlate);
public sealed record CreateReservationRestaurantDto(
    int TableId,
    DateTime TableReservationStart,
    DateTime TableReservationEnd,
    decimal TableBill,
    decimal TableDiscount
);
