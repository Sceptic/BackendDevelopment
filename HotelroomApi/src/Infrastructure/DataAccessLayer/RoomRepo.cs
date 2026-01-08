using System.Data;
using Domain.Models;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DataAccess;

internal partial class HotelroomRepository
{
    private static async Task<IReadOnlyList<Hotelroom>> LoadAllRoomsAsync(SqlConnection connection)
    {
        var rooms = new List<Hotelroom>();

        var sql = @"
            SELECT RoomId,
                   RoomNumber,
                   HotelroomPrice,
                   IsAvailable,
                   CapacityMin,
                   CapacityMax
            FROM tblHotelroom";

        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            rooms.Add(MapRoom(reader));
        }

        return rooms;
    }

    private static async Task<Hotelroom?> LoadAggregateAsync(SqlConnection connection, int roomId)
    {
        var room = await LoadRoomAsync(connection, roomId);
        if (room is null) return null;

        await LoadAmenitiesForRoomAsync(connection, room);
        await LoadBedsForRoomAsync(connection, room);

        return room;
    }

    private static async Task<Hotelroom?> LoadRoomAsync(SqlConnection connection, int roomId)
    {
        var sql = @"
            SELECT RoomId,
                   RoomNumber,
                   HotelroomPrice,
                   IsAvailable,
                   CapacityMin,
                   CapacityMax
            FROM tblHotelroom
            WHERE RoomId = @RoomId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@RoomId", SqlDbType.Int).Value = roomId;

        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        return MapRoom(reader);
    }

    private static Hotelroom MapRoom(SqlDataReader reader)
    {
        var roomId = reader.GetInt32(reader.GetOrdinal("RoomId"));
        var roomNumber = reader.GetInt32(reader.GetOrdinal("RoomNumber"));

        decimal? hotelroomPrice = reader.IsDBNull(reader.GetOrdinal("HotelroomPrice"))
            ? null
            : reader.GetDecimal(reader.GetOrdinal("HotelroomPrice"));

        bool? isAvailable = reader.IsDBNull(reader.GetOrdinal("IsAvailable"))
            ? (bool?)null
            : reader.GetBoolean(reader.GetOrdinal("IsAvailable"));

        int? capacityMin = reader.IsDBNull(reader.GetOrdinal("CapacityMin"))
            ? (int?)null
            : reader.GetInt32(reader.GetOrdinal("CapacityMin"));

        int? capacityMax = reader.IsDBNull(reader.GetOrdinal("CapacityMax"))
            ? (int?)null
            : reader.GetInt32(reader.GetOrdinal("CapacityMax"));

        return Hotelroom.Rehydrate(
            roomId,
            roomNumber,
            hotelroomPrice,
            isAvailable,
            capacityMin,
            capacityMax
        );
    }
}
