using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Api.IntegrationTests;

public sealed partial class ReservationApiSmokeTests
{
    private static async Task RecreateDatabaseAsync(string dbName)
    {
        var masterCs = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

        await using var conn = new SqlConnection(masterCs);
        await conn.OpenAsync();

        var sql = $@"
                    IF DB_ID(N'{dbName}') IS NOT NULL
                    BEGIN
                        ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        DROP DATABASE [{dbName}];
                    END;

                    CREATE DATABASE [{dbName}];
                    ";
        await using var cmd = new SqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task DropDatabaseAsync(string dbName)
    {
        var masterCs = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

        await using var conn = new SqlConnection(masterCs);
        await conn.OpenAsync();

        var sql = $@"
                    IF DB_ID(N'{dbName}') IS NOT NULL
                    BEGIN
                        ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        DROP DATABASE [{dbName}];
                    END;
                    ";
        await using var cmd = new SqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}
