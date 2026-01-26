using Application.Abstractions.Persistence;
using Domain.Models;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DataAccess;

internal partial class HotelroomRepository : IHotelroomRepository
{
    private readonly string _connectionString;

    internal HotelroomRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IReadOnlyList<Hotelroom>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        return await LoadAllRoomsAsync(connection);
    }

    public async Task<Hotelroom?> GetByIdAsync(int roomId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        return await LoadAggregateAsync(connection, roomId);
    }
}