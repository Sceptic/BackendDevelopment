using Application.Abstractions;
using Application.Abstractions.Reservations;
using Domain.ErrorHandling;
using Domain.Models;

namespace Application.Reservations;

public sealed class ReservationExternalPolicy : IReservationExternalPolicy
{
    private readonly ICampingReadClient _campings;
    private readonly IGiteReadClient _gites;
    private readonly IHotelroomReadClient _rooms;
    private readonly IRestaurantReadClient _restaurant;

    public ReservationExternalPolicy(
        ICampingReadClient campings,
        IGiteReadClient gites,
        IHotelroomReadClient rooms,
        IRestaurantReadClient restaurant)
    {
        _campings = campings;
        _gites = gites;
        _rooms = rooms;
        _restaurant = restaurant;
    }

    public async Task ApplyAsync(Reservation reservation, CancellationToken ct)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        //Determine the amount of people registered in the reservation
        var people = reservation.Clients?.Count ?? 0;

        //Retrieves information from the external APIs as to given accommodation: exists, what its availability is and its capacity
        var giteTasks = reservation.Gites
            .Select(async x => (id: x.GiteId, snapshot: await SafeGetGiteAsync(x.GiteId, errors, ct)))
            .ToArray();

        var roomTasks = reservation.Hotelrooms
            .Select(async x => (id: x.RoomId, snapshot: await SafeGetRoomAsync(x.RoomId, errors, ct)))
            .ToArray();

        var campingTasks = reservation.Campings
            .Select(async x => (id: x.CampingId, snapshot: await SafeGetCampingAsync(x.CampingId, errors, ct)))
            .ToArray();

        var tableTasks = reservation.Restaurants
            .Select(async x => (id: x.TableId, snapshot: await SafeGetTableAsync(x.TableId, errors, ct)))
            .ToArray();

        var gites = await Task.WhenAll(giteTasks);
        var rooms = await Task.WhenAll(roomTasks);
        _ = await Task.WhenAll(campingTasks);
        var tables = await Task.WhenAll(tableTasks);

        //Checks if the accommodation even exists
        if (errors.Count > 0)
            throw new DomainValidationException("Validation failed", errors);

        //Checks if it is available
        foreach (var g in gites)
        {
            if (!g.snapshot.IsAvailable)
                Add(errors, "gites", $"Gite {g.id} is not available.");
        }

        foreach (var r in rooms)
        {
            if (!r.snapshot.IsAvailable)
                Add(errors, "hotelrooms", $"Hotelroom {r.id} is not available.");
        }

        //Checks if the given amount of people in the reservation can fit into the reserved accommodation,
        //furthermore reservations which have less than one person or don't have any reservations also get failed here.
        var totalMin = gites.Sum(x => x.snapshot.CapacityMin) + rooms.Sum(x => x.snapshot.CapacityMin);
        var totalMax = gites.Sum(x => x.snapshot.CapacityMax) + rooms.Sum(x => x.snapshot.CapacityMax);

        if (people <= 0)
            Add(errors, "clients", "At least one client is required.");

        if (people > 0 && (totalMax == 0))
            Add(errors, "accommodations", "At least one accommodation (gite or hotelroom) is required to validate capacity.");

        if (people > 0 && totalMax > 0 && people > totalMax)
            Add(errors, "capacity", $"Reservation has {people} people but accommodation capacity max is {totalMax}.");

        foreach (var t in tables)
        {
            if (people > t.snapshot.Capacity)
                Add(errors, "restaurants", $"Table {t.id} capacity is {t.snapshot.Capacity} but reservation has {people} people.");
        }

        //Computes the total price of the reservation, excluding the camping as the casegroup of LeftoverGPTeam did not properly 
        //provide a price in their GET-method, as such it could not be implemented in the summation
        var computedAccommodationPrice =
            gites.Sum(x => x.snapshot.GitePrice) +
            rooms.Sum(x => x.snapshot.HotelroomPrice) +
            reservation.Restaurants.Sum(x => x.TableBill);

        reservation.ReservationPrice = computedAccommodationPrice;

        if (errors.Count > 0)
            throw new DomainValidationException("Validation failed", errors);
    }

    //Following methods access the external APIs via interface ports facing the infrastructure
    private async Task<GiteSnapshot> SafeGetGiteAsync(int giteId, Dictionary<string, string[]> errors, CancellationToken ct)
    {
        try
        {
            return await _gites.GetInfoAsync(new GiteRequest(giteId), ct);
        }
        catch (Exception)
        {
            Add(errors, "gites", $"Gite {giteId} does not exist or could not be retrieved.");
            return new GiteSnapshot(giteId, 0m, false, 0, 0);
        }
    }

    private async Task<HotelroomSnapshot> SafeGetRoomAsync(int roomId, Dictionary<string, string[]> errors, CancellationToken ct)
    {
        try
        {
            return await _rooms.GetInfoAsync(new HotelroomRequest(roomId), ct);
        }
        catch (Exception)
        {
            Add(errors, "hotelrooms", $"Hotelroom {roomId} does not exist or could not be retrieved.");
            return new HotelroomSnapshot(roomId, 0m, false, 0, 0);
        }
    }

    private async Task<CampingSnapshot> SafeGetCampingAsync(int campingId, Dictionary<string, string[]> errors, CancellationToken ct)
    {
        try
        {
            return await _campings.GetInfoAsync(new CampingRequest(campingId), ct);
        }
        catch (Exception)
        {
            Add(errors, "campings", $"Camping {campingId} does not exist or could not be retrieved.");
            return new CampingSnapshot(campingId);
        }
    }

    private async Task<RestaurantSnapshot> SafeGetTableAsync(int tableId, Dictionary<string, string[]> errors, CancellationToken ct)
    {
        try
        {
            return await _restaurant.GetInfoAsync(new RestaurantRequest(tableId), ct);
        }
        catch (Exception)
        {
            Add(errors, "restaurants", $"Restaurant table {tableId} does not exist or could not be retrieved.");
            return new RestaurantSnapshot(tableId, null);
        }
    }

    private static void Add(Dictionary<string, string[]> errors, string key, string message)
    {
        if (!errors.TryGetValue(key, out var arr))
        {
            errors[key] = new[] { message };
            return;
        }

        var list = arr.ToList();
        list.Add(message);
        errors[key] = list.ToArray();
    }
}
