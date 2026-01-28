using LegacyMonolith.Data;
using LegacyMonolith.Models;
using Microsoft.AspNetCore.Mvc;

//Beschrijft de API die externe applicaties gebruiken om met deze applicatie te communiceren. Een API maakt een UOW (Unit of work) aan om data op te halen en schrijft deze naar een Dto,
//die dan omgezet wordt naar een .json bestand die opgestuurd wordt via de API.

namespace LegacyMonolith.Controllers
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
