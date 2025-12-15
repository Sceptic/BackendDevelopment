using HotelProgramma.Data;
using HotelProgramma.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelProgramma.Controllers
{
    [ApiController]
    [Route("api/gites")]
    public class GitesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public GitesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{giteNumber}")]
        public ActionResult<GiteDto> Get(int giteNumber)
        {
            var gite = _uow.Gites.Get(giteNumber);

            if (gite == null)
                return NotFound();

            var dto = new GiteDto
            {
                GiteNumber = gite.GiteNumber,
                GitePrice = gite.GitePrice,
                IsAvailable = gite.IsAvailable,
                GiteAddress = gite.GiteAddress,
                CapacityMin = gite.CapacityMin,
                CapacityMax = gite.CapacityMax
            };

            return Ok(dto);
        }
    }
}
