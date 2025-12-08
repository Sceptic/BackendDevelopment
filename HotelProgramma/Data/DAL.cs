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
        // Connection string naar de SQL Server database
        // Data Source = naam van de SQL server instance.
        // Initial Catalog = naam van de database.
        // Integrated Security = gebruikt Windows-authenticatie.
        // Trust Server Certificate = accepteert het servercertificaat zonder extra validatie.
        // Staat op private readonly om te preventeren dat de variabele _connectionString herschreven wordt.
        private readonly string _connectionString =
            "Data Source=VIVOBOOK\\SQLEXPRESS;Initial Catalog=marconnes_db;Integrated Security=True;Trust Server Certificate=True";

        /// <summary>
        ///  Haalt alle hotelkamers uit de database op.
        ///  Retourneert een lijst van Room objecten met kamernummer, prijs en beschikbaarheid.
        /// </summary>
        public List<Room> GetAllRooms()
        {
            // Maakt een lege lijst aan waar alle kamers in verzameld worden.
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
