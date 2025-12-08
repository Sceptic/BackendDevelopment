using HotelProgramma.Models;
using Microsoft.Data.SqlClient; // Maakt de SQL Server types beschikbaar. Denk aan SqlConnection, SqlCommand, SqlDataReader, etc.

namespace HotelProgramma.Data
{
    /// <summary>
    /// Data Access Layer (DAL) klasse.
    /// Verantwoordelijk voor communicatie met de database van het hotelprogramma
    /// </summary>
    public class DAL
    {
        private readonly string _connectionString;
        
        public DAL()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();

            // Maakt een nieuwe SQL connectie aan met de opgegeven connection string uit de _connectionString variabele.
            using (SqlConnection conn = new SqlConnection(_connectionString))

            // Maakt een SQL-command aan met de query en de connectie.
            // // De query selecteert drie kolommen uit de tabel HOTELROOM.
            using (SqlCommand cmd = new SqlCommand(
                "SELECT room_number, hotelroom_price, is_available FROM HOTELROOM",
                conn))                                     
            {

                // Opent de verbinding met de database.
                conn.Open();                               

                // Voert de SQL query uit en krijgt een SqlDataReader terug.
                // De reader wordt gebruikt om rij voor rij door de resultaten te lopen.
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    // Zolang er nog rijen zijn in het resultaat: doorgaan.
                    while (reader.Read())
                    {

                        // Maakt een nieuwe Room object en vult de properties.
                        // reader.GetInt32(0) leest de eerste kolom (room_number) als integer.
                        // reader.GetInt32(1) leest de tweede kolom (hotelroom_price) als integer.
                        // reader.GetBoolean(2) leest de derde kolom (is_available) als boolean.
                        var room = new Room
                        {
                            RoomNumber = reader.GetInt32(0),
                            HotelRoomPrice = reader.GetInt32(1),
                            IsAvailable = reader.GetBoolean(2)
                        };

                        Console.WriteLine(room);
                        // Voegt de ingevulde kamer toe aan de lijst
                        rooms.Add(room);
                    }
                } // SqlDataReader wordt hier automatisch gesloten.
            } // SqlCommand en SqlConnection worden hier automatisch opgeruimd en de connectie wordt gesloten totdat het weer nodig is.

            // Geeft de volledige lijst met kamers terug aan de aanroeper.
            return rooms;
        }
    }
}
