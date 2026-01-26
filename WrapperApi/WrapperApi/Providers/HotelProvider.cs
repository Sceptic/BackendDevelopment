using WrapperApi.Contracts;
using WrapperApi.Wrapper;

namespace WrapperApi.Providers;

public sealed class HotelProvider : ICatalogProvider
{
    public SourceSystem Source => SourceSystem.Hotel;
    private readonly DalApiClient _dal;

    public HotelProvider(DalApiClient dal) => _dal = dal;

    public async Task<IReadOnlyList<AccommodationCard>> GetCatalog(CancellationToken ct)
    {
        var rooms = await _dal.GetHotelRooms(ct);

        return rooms.Select(r => new AccommodationCard(
            Source: SourceSystem.Hotel,
            AccommodationId: r.RoomId.ToString(),
            Name: $"Room {r.RoomNumber}",
            Type: "Hotelroom",
            CapacityMin: r.CapacityMin ?? 0,
            CapacityMax: r.CapacityMax ?? 0,
            PricePerNight: r.HotelroomPrice ?? 0m,
            Currency: "EUR",
            Available: r.IsAvailable ?? false
        )).ToList();
    }

    public async Task<IReadOnlyList<AccommodationCard>> GetAvailability(AvailabilityQuery q, CancellationToken ct)
    {
        // bron heeft geen datum-availability, dus dit is catalog + filters
        var items = (await GetCatalog(ct)).AsEnumerable();

        if (q.Guests is not null)
            items = items.Where(x => x.CapacityMin <= q.Guests && x.CapacityMax >= q.Guests);

        if (q.CapacityMin is not null) items = items.Where(x => x.CapacityMax >= q.CapacityMin);
        if (q.CapacityMax is not null) items = items.Where(x => x.CapacityMin <= q.CapacityMax);

        return items.ToList();
    }
}
