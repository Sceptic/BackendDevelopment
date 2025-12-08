namespace HotelProgramma.Models
{

    /// <summary>
    /// Stelt één hotelkamer voor met nummer, prijs en beschikbaarheid.
    /// Dit is een puur data-object zonder logica.
    /// </summary>
    public class Room
    {

        /// <summary>
        /// Eigenschappen van een hotelkamer met een getter en setter
        /// om de eigenschap te kunnen ophalen (get) en te herschrijven (set).
        /// </summary>
        public int RoomNumber {  get; set; }
        public int HotelRoomPrice {  get; set; }
        public bool IsAvailable { get; set; }
    }
}
