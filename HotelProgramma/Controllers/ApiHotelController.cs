using HotelProgramma.Data;
using HotelProgramma.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelProgramma.Controllers
{
    [ApiController]
    [Route("api/hotelrooms")]
    public class HotelRoomsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public HotelRoomsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{roomNumber}")]
        public ActionResult<HotelRoomDto> Get(int roomNumber)
        {
            var room = _uow.Hotels.Get(roomNumber);

            //Een hotelkamer moet ook een bedconfiguratie en amenities hebben, als deze missen wordt hier een error gegooid.
            if (room == null)
                return NotFound();

            if (room.Bed == null)
                throw new Exception(
                    $"HotelRoom {room.RoomNumber} has no Bed configured.");

            if (room.Amenities == null)
                throw new Exception(
                    $"HotelRoom {room.RoomNumber} has no Amenities configured.");

            var dto = new HotelRoomDto
            {
                RoomNumber = room.RoomNumber,
                HotelroomPrice = room.HotelroomPrice,
                IsAvailable = room.IsAvailable,
                CapacityMin = room.CapacityMin,
                CapacityMax = room.CapacityMax,

                Bed = new HotelRoomBedDto
                {
                    Amount1PrBed = room.Bed.Amount1PrBed,
                    Amount2PrBed = room.Bed.Amount2PrBed,
                    Amount3PrBed = room.Bed.Amount3PrBed,
                    BedSort = room.Bed.BedSort
                },

                Amenities = new HotelRoomAmenitiesDto
                {
                    Wifi = room.Amenities.Wifi,
                    Bath = room.Amenities.Bath,
                    Shower = room.Amenities.Shower,
                    Hairdryer = room.Amenities.Hairdryer,
                    Smallchild = room.Amenities.Smallchild,
                    Toiletries = room.Amenities.Toiletries,
                    Desk = room.Amenities.Desk,
                    Chair = room.Amenities.Chair,
                    Balcony = room.Amenities.Balcony,
                    Sofa = room.Amenities.Sofa,
                    Sofabed = room.Amenities.Sofabed,
                    Minifridge = room.Amenities.Minifridge,
                    Kettle = room.Amenities.Kettle,
                    Cuttlery = room.Amenities.Cuttlery,
                    Eatingarea = room.Amenities.Eatingarea,
                    Roomservice = room.Amenities.Roomservice
                }
            };

            return Ok(dto);
        }
    }
}
