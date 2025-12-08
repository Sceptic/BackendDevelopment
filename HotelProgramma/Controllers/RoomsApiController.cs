using System.Collections.Generic;
using HotelProgramma.Data;
using HotelProgramma.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelProgramma.Controllers
{

    // Deze controller is een Web API
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsApiController : ControllerBase
    {
        private readonly DAL _dal;

        public RoomsApiController(DAL dal)
        {
            _dal = dal;
        }

        // GET: /api/roomsapi
        [HttpGet]
        public ActionResult<List<Room>> Get()
        {
            var rooms = _dal.HotelRoomGetAll();
            return Ok(rooms); // Swagger ziet dit als een GET-operatie.
        }
    }
}
