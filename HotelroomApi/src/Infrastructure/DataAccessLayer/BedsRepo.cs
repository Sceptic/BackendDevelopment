using System.Data;
using Domain.Models;
using Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DataAccess;

internal partial class HotelroomRepository
{
    private static async Task LoadBedsForRoomAsync(SqlConnection connection, Hotelroom room)
    {
        var sql = @"
            SELECT HotelroomBedId,
                   RoomId,
                   Amount1PrBed,
                   Amount2PrBed,
                   Amount3PrBed,
                   BedSort
            FROM tblHotelroomBed
            WHERE RoomId = @RoomId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@RoomId", SqlDbType.Int).Value = room.RoomId;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var hotelroomBedId = reader.GetInt32(reader.GetOrdinal("HotelroomBedId"));
            var roomId = reader.GetInt32(reader.GetOrdinal("RoomId"));
            var amount1 = reader.GetNullableInt("Amount1PrBed");
            var amount2 = reader.GetNullableInt("Amount2PrBed");
            var amount3 = reader.GetNullableInt("Amount3PrBed");
            var bedSort = reader.IsDBNull(reader.GetOrdinal("BedSort"))
                ? null
                : reader.GetString(reader.GetOrdinal("BedSort"));

            var bed = HotelroomBed.Rehydrate(
                hotelroomBedId,
                roomId,
                room,
                amount1,
                amount2,
                amount3,
                bedSort
            );

            room.AddBed(bed);
        }
    }
}
