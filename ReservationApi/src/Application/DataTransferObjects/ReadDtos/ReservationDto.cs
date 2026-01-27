namespace Application.DTOs.Reservations;

public sealed record ReservationDto
(
    int ReservationId,
    int AccountId,
    string ReservationStatus,
    string PaymentStatus,
    decimal ReservationPrice,
    decimal Discount,
    decimal TouristTarif,
    DateTime ReservationStart,
    DateTime ReservationEnd,
    IReadOnlyList<ReservationClientDto> Clients,
    IReadOnlyList<ReservationGiteDto> Gites,
    IReadOnlyList<ReservationHotelroomDto> Hotelrooms,
    IReadOnlyList<ReservationCampingDto> Campings,
    IReadOnlyList<ReservationFacilityDto> Facilities,
    IReadOnlyList<VehicleDto> Vehicles,
    IReadOnlyList<ReservationRestaurantDto> Restaurants
);
