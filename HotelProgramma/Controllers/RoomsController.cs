using System.Net.Http.Headers;
using HotelProgramma.Data;
using Microsoft.AspNetCore.Mvc; // Nodig voor Controller, IActionResult, View(), etc.

namespace HotelProgramma.Controllers
{

    /// <summary>
    /// Controller die verantwoordelijk is voor alles rond hotelkamers.
    /// Behandelt HTTP-verzoeken voor /Rooms/...
    /// </summary>
    public class RoomsController : Controller
    {

        // Veld om een instantie van de DAL (Data Access Layer) op te slaan.
        // Via deze DAL praat de controller met de database.
        private readonly DAL _dal;

        /// <summary>
        /// Constructor van de RoomsController.
        /// De Dal wordt via dependency injection binnengehaald.
        /// ASP.NET Core maakt zelf een DAL-object aan en geeft deze hier mee.
        /// </summary>
        /// <param name="dal">De geïnjecteerde Data Access Layer.</param>
        public RoomsController(DAL dal)
        {
            _dal = dal; // Slaat de meegegeven DAL op in het private variabele zodat andere methods het kunnen gebruiken.
        }

        /// <summary>
        /// Acties voor de URL: /Rooms of /Rooms/Index
        /// Haalt alle kamers op uit de database en geeft ze door aan de view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            // Vraagt via de DAL alle kamers op uit de database.
            // 'rooms' is hier een List<Room>.
            var rooms = _dal.GetAllRooms();

            // Geeft de 'Rooms' lijst mee aan de view.
            // De view ontvangt dit als het model (bijvoorbeeld @model List<Room>).
            return View(rooms);
        }
    }
}
