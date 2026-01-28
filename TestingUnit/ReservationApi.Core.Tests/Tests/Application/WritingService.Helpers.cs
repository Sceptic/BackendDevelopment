using Application.DTOs.Reservations;

namespace ReservationApi.Core.Tests.Application;

public sealed partial class ReservationApplicationTests
{
    private static CreateReservationRequestDto MakeCreateRequest(
        int accountId = 1001,
        string reservationStatus = "CONFIRMED",
        string paymentStatus = "PAID",
        decimal price = 1200m,
        decimal discount = 0.10m,
        decimal touristTarif = 0.05m,
        DateTime? start = null,
        DateTime? end = null)
    {
        var s = start ?? new DateTime(2026, 06, 01, 14, 00, 00);
        var e = end ?? new DateTime(2026, 06, 10, 10, 00, 00);

        return new CreateReservationRequestDto(
            AccountId: accountId,
            ReservationStatus: reservationStatus,
            PaymentStatus: paymentStatus,
            ReservationPrice: price,
            Discount: discount,
            TouristTarif: touristTarif,
            ReservationStart: s,
            ReservationEnd: e,
            Clients: new[]
            {
                new CreateReservationClientDto("Anna", "Peeters", new DateTime(1985, 03, 22)),
            },
            Gites: new[]
            {
                new CreateReservationGiteDto(501, 0.15m),
            },
            Hotelrooms: new[]
            {
                new CreateReservationHotelroomDto(301, 0.05m),
            },
            Campings: new[]
            {
                new CreateReservationCampingDto(1, 0.20m),
            },
            Facilities: new[]
            {
                new CreateReservationFacilityDto("Sauna", 0.15m),
            },
            Vehicles: new[]
            {
                new CreateVehicleDto("1-ABC-123"),
            },
            Restaurants: new[]
            {
                new CreateReservationRestaurantDto(
                    TableId: 10,
                    TableReservationStart: s.AddHours(4),
                    TableReservationEnd: s.AddHours(6),
                    TableBill: 140m,
                    TableDiscount: 0.10m),
            }
        );
    }

    private static PatchReservationRequestDto MakePatchRequest(
        int reservationId,
        int? accountId = null,
        string? reservationStatus = null,
        string? paymentStatus = null,
        decimal? price = null,
        decimal? discount = null,
        decimal? touristTarif = null,
        DateTime? start = null,
        DateTime? end = null)
    {
        return new PatchReservationRequestDto(
            ReservationId: reservationId,
            AccountId: accountId,
            ReservationStatus: reservationStatus,
            PaymentStatus: paymentStatus,
            ReservationPrice: price,
            Discount: discount,
            TouristTarif: touristTarif,
            ReservationStart: start,
            ReservationEnd: end,
            Clients: null,
            Gites: null,
            Hotelrooms: null,
            Campings: null,
            Facilities: null,
            Vehicles: null,
            Restaurants: null
        );
    }
}
