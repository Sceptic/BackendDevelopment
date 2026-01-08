using System.Data;
using Domain.Models;
using Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DataAccess;

internal partial class HotelroomRepository
{
    private static async Task LoadAmenitiesForRoomAsync(SqlConnection connection, Hotelroom room)
    {
        var sql = @"
            SELECT RoomId,
                   Wifi,
                   Bath,
                   Shower,
                   Hairdryer,
                   Smallchild,
                   Toiletries,
                   Desk,
                   Chair,
                   Balcony,
                   Sofa,
                   Sofabed,
                   Minifridge,
                   Kettle,
                   Cuttlery,
                   Eatingarea,
                   Roomservice
            FROM tblHotelroomAmenities
            WHERE RoomId = @RoomId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@RoomId", SqlDbType.Int).Value = room.RoomId;

        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return;

        var amenities = HotelroomAmenities.Rehydrate(
            room.RoomId,
            room,
            reader.GetNullableBool("Wifi"),
            reader.GetNullableBool("Bath"),
            reader.GetNullableBool("Shower"),
            reader.GetNullableBool("Hairdryer"),
            reader.GetNullableBool("Smallchild"),
            reader.GetNullableBool("Toiletries"),
            reader.GetNullableBool("Desk"),
            reader.GetNullableBool("Chair"),
            reader.GetNullableBool("Balcony"),
            reader.GetNullableBool("Sofa"),
            reader.GetNullableBool("Sofabed"),
            reader.GetNullableBool("Minifridge"),
            reader.GetNullableBool("Kettle"),
            reader.GetNullableBool("Cuttlery"),
            reader.GetNullableBool("Eatingarea"),
            reader.GetNullableBool("Roomservice")
        );

        room.AttachAmenities(amenities);
    }
}
