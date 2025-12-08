using HotelProgramma.Models;
using Microsoft.Data.SqlClient;
using System.Dynamic;

namespace HotelProgramma.Data
{
    public class DAL
    {
        private readonly string _connectionString;
        
        //Constructor retrieves the connectionstring from the appsettings.json file at runtime, ensure the relevant connectionstring is there.
        public DAL()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Room> HotelRoomGetAll()
        {
            var rooms = new List<Room>();

            using (SqlConnection conn = new SqlConnection(_connectionString))

            using (SqlCommand cmd = new SqlCommand(
            "SELECT room_number, hotelroom_price, is_available FROM HOTELROOM",
            conn))                                     
            {
                conn.Open();                               

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var room = new Room
                        {
                            RoomNumber = reader.GetInt32(0),
                            HotelRoomPrice = reader.GetInt32(1),
                            IsAvailable = reader.GetBoolean(2)
                        };

                        Console.WriteLine(room);
                        rooms.Add(room);
                    }
                }
            }
            return rooms;
        }

        //public List<T> SqlRetrieve<T>(string sql, Action<SqlCommand> bindParams, Func<SqlDataReader, T> map)

        //{

        //}

        //public void GetAllHotelRoomsOnBoolAvailability(bool availabilityFilter)
        //{
        //    CreateQuery("SELECT room_number, hotelroom_price, is_available FROM HOTELROOM WHERE is_available = @avail");
        //}


        public List<Room> QueryExample()
        {
            return SqlGetPipeline<Room>(
                "SELECT room_number, hotelroom_price, is_available FROM HOTELROOM WHERE is_available = @avail",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@avail", true);
                },
                reader => new Room
                {
                    RoomNumber = reader.GetInt32(0),
                    HotelRoomPrice = reader.GetInt32(1),
                    IsAvailable = reader.GetBoolean(2)
                }
            );
        }

        public List<T> SqlGetPipeline<T>(string sql, Action<SqlCommand> queryParameters, Func<SqlDataReader, T> classModelMap)
        {
            var results = new List<T>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                queryParameters?.Invoke(cmd);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(classModelMap(reader));
                    }
                }
            }
            return results;
        }
    }
}
