namespace FullSystemIntegrationTesting;

internal static class TestConfig
{
    public static Uri HotelroomApiBaseUrl => new("https://localhost:7001");
    public static Uri GiteApiBaseUrl => new("https://localhost:7002");
    public static Uri ReservationApiBaseUrl => new("https://localhost:7003");

    public static int ReservationTestGiteId => 1;
}
