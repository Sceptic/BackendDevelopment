using HotelProgramma.Data;
using HotelProgramma.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelProgramma.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ReservationsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{id}")]
        public ActionResult<ReservationDto> Get(int id)
        {
            var reservation = _uow.Reservations.Get(id);

            if (reservation == null)
                return NotFound();

            var dto = new ReservationDto
            {
                ReservationId = reservation.ReservationId,
                AccountId = reservation.AccountId,

                ReservationStatus = reservation.ReservationStatus,
                PaymentStatus = reservation.PaymentStatus,

                Discount = reservation.Discount,
                ReservationStart = reservation.ReservationStart,
                ReservationEnd = reservation.ReservationEnd,


                Clients = reservation.ReservationClient?
                    .Select(c => new ReservationClientDto
                    {
                        ReservationId = reservation.ReservationId,
                        Firstname = c.Firstname,
                        Lastname = c.Lastname,
                        Birthdate = c.Birthdate
                    })
                    .ToList() ?? new List<ReservationClientDto>(),

                Hotels = reservation.ReservationHotel?
                    .Select(h => new ReservationHotelDto
                    {
                        ReservationId = reservation.ReservationId,
                        RoomNumber = h.RoomNumber,
                        HotelroomDiscount = h.HotelroomDiscount
                    })
                    .ToList() ?? new List<ReservationHotelDto>(),

                Gites = reservation.ReservationGite?
                    .Select(g => new ReservationGiteDto
                    {
                        ReservationId = reservation.ReservationId,
                        GiteNumber = g.GiteNumber,
                        GiteDiscount = g.GiteDiscount
                    })
                    .ToList() ?? new List<ReservationGiteDto>()
            };

            return Ok(dto);
        }
    }
}
