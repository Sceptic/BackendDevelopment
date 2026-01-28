namespace WrapperApi.Wrapper;

// DTO's 1-op-1 overgenomen uit LegacyMonolith (Models/ApiModels/ReservationApiModel.cs)
// Hiermee blijft je JSON contract exact gelijk aan wat jullie eerder al hadden.

public sealed class ReservationDto
{
    public int ReservationId { get; set; }
    public int AccountId { get; set; }

    public string ReservationStatus { get; set; } = "";
    public string PaymentStatus { get; set; } = "";

    public int Discount { get; set; }
    public DateTime ReservationStart { get; set; }
    public DateTime ReservationEnd { get; set; }

    public List<ReservationClientDto> Clients { get; set; } = new();
    public List<ReservationHotelDto> Hotels { get; set; } = new();
    public List<ReservationGiteDto> Gites { get; set; } = new();
}

public sealed class ReservationClientDto
{
    public int ReservationId { get; set; }
    public string Firstname { get; set; } = "";
    public string Lastname { get; set; } = "";
    public DateTime Birthdate { get; set; }
}

public sealed class ReservationHotelDto
{
    public int ReservationId { get; set; }
    public int RoomNumber { get; set; }
    public int HotelroomDiscount { get; set; }
}

public sealed class ReservationGiteDto
{
    public int ReservationId { get; set; }
    public int GiteNumber { get; set; }
    public int GiteDiscount { get; set; }
}

// Lichte modellen voor availability (GetAll responses) 
// Deze zijn expres "tolerant" (nullable) omdat je huidige get-all output bv. amenities:null en beds:[] bevat.
public sealed class HotelRoomListItemDto
{
    public int RoomId { get; set; }
    public int RoomNumber { get; set; }
    public decimal? HotelroomPrice { get; set; }
    public bool? IsAvailable { get; set; }
    public int? CapacityMin { get; set; }
    public int? CapacityMax { get; set; }
}

public sealed class GiteListItemDto
{
    public int GiteId { get; set; }
    public int GiteNumber { get; set; }
    public decimal? GitePrice { get; set; }
    public bool? IsAvailable { get; set; }
    public int? CapacityMin { get; set; }
    public int? CapacityMax { get; set; }
}

public sealed class AccountDto
{
    public int AccountId { get; set; }
}
