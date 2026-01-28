using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace Api.IntegrationTests;

public sealed class HotelroomApiSmokeTests : IClassFixture<HotelroomApiFactory>, IAsyncLifetime
{
    private readonly HotelroomApiFactory _factory;
    private readonly HttpClient _client;

    public HotelroomApiSmokeTests(HotelroomApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await CreateCleanDatabaseAsync(_factory.DatabaseName);
        await CreateSchemaAndSeedAsync(_factory.ConnectionString);
    }

    public async Task DisposeAsync()
    {
        await DropDatabaseAsync(_factory.DatabaseName);
    }

    [Fact]
    public async Task Smoke_hits_all_endpoints()
    {
        //GET /hotelroom
        var allResp = await _client.GetAsync("/hotelroom");
        Assert.Equal(HttpStatusCode.OK, allResp.StatusCode);

        using (var doc = JsonDocument.Parse(await allResp.Content.ReadAsStringAsync()))
        {
            Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.True(doc.RootElement.GetArrayLength() >= 1);
        }

        //GET /hotelroom/{id}
        var byIdResp = await _client.GetAsync("/hotelroom/1");
        Assert.Equal(HttpStatusCode.OK, byIdResp.StatusCode);

        //GET /hotelroom/{id} not found
        var missing = await _client.GetAsync("/hotelroom/999999");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    private static async Task CreateCleanDatabaseAsync(string dbName)
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

    private static async Task CreateSchemaAndSeedAsync(string cs)
    {
        await using var conn = new SqlConnection(cs);
        await conn.OpenAsync();

        var createTables = @"
                            CREATE TABLE tblHotelroom (
                                RoomId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                RoomNumber INT NOT NULL,
                                HotelroomPrice DECIMAL(5,2) NULL,
                                IsAvailable BIT NULL,
                                CapacityMin INT NULL,
                                CapacityMax INT NULL
                            );

                            CREATE TABLE tblHotelroomAmenities (
                                RoomId INT NOT NULL PRIMARY KEY,
                                Wifi BIT NULL,
                                Bath BIT NULL,
                                Shower BIT NULL,
                                Hairdryer BIT NULL,
                                Smallchild BIT NULL,
                                Toiletries BIT NULL,
                                Desk BIT NULL,
                                Chair BIT NULL,
                                Balcony BIT NULL,
                                Sofa BIT NULL,
                                Sofabed BIT NULL,
                                Minifridge BIT NULL,
                                Kettle BIT NULL,
                                Cuttlery BIT NULL,
                                Eatingarea BIT NULL,
                                Roomservice BIT NULL,
                                CONSTRAINT FK_tblHotelroomAmenities_tblHotelroom
                                    FOREIGN KEY (RoomId) REFERENCES tblHotelroom(RoomId) ON DELETE CASCADE
                            );

                            CREATE TABLE tblHotelroomBed (
                                HotelroomBedId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                RoomId INT NOT NULL,
                                Amount1PrBed INT NULL,
                                Amount2PrBed INT NULL,
                                Amount3PrBed INT NULL,
                                BedSort TEXT NULL,
                                CONSTRAINT FK_tblHotelroomBed_tblHotelroom
                                    FOREIGN KEY (RoomId) REFERENCES tblHotelroom(RoomId) ON DELETE CASCADE
                            );

                            CREATE UNIQUE INDEX IX_tblHotelroom_RoomNumber ON tblHotelroom(RoomNumber);
                            CREATE INDEX IX_tblHotelroomBed_RoomId ON tblHotelroomBed(RoomId);
                            ";

        await using (var cmd = new SqlCommand(createTables, conn))
            await cmd.ExecuteNonQueryAsync();

        var seed = @"
                    INSERT INTO tblHotelroom (RoomNumber, HotelroomPrice, IsAvailable, CapacityMin, CapacityMax)
                    VALUES (101, 99.99, 1, 1, 2);

                    DECLARE @RoomId INT = SCOPE_IDENTITY();

                    INSERT INTO tblHotelroomAmenities
                    (RoomId, Wifi, Bath, Shower, Hairdryer, Smallchild, Toiletries, Desk, Chair, Balcony, Sofa, Sofabed, Minifridge, Kettle, Cuttlery, Eatingarea, Roomservice)
                    VALUES
                    (@RoomId, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, NULL);

                    INSERT INTO tblHotelroomBed (RoomId, Amount1PrBed, Amount2PrBed, Amount3PrBed, BedSort)
                    VALUES (@RoomId, 1, 0, 0, 'Single');
                    ";

        await using (var cmd = new SqlCommand(seed, conn))
            await cmd.ExecuteNonQueryAsync();
    }
}
