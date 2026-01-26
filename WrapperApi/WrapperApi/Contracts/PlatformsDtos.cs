namespace WrapperApi.Contracts;

public enum SourceSystem { Hotel, Gite, Camping }

public sealed record AvailabilityQuery(
    DateTime Start,
    DateTime End,
    int? Guests = null,
    int? CapacityMin = null,
    int? CapacityMax = null,
    SourceSystem? Source = null
);

public sealed record AccommodationCard(
    SourceSystem Source,
    string AccommodationId,
    string Name,
    string Type,
    int CapacityMin,
    int CapacityMax,
    decimal PricePerNight,
    string Currency,
    bool Available
);

public sealed record CreateReservationRequest(
    SourceSystem Source,
    string AccommodationId,
    int AccountId,
    DateTime Start,
    DateTime End,
    int Guests,
    string? Notes = null,
    string? IdempotencyKey = null
);

public sealed record ReservationCreatedResponse(
    int PlatformReservationId,
    SourceSystem Source,
    string SourceReservationId
);