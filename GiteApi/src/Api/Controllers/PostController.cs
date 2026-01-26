using Application.DtoModels;
using Application.Gites.WriteQueries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("gite/post")]
    public sealed class PostGiteController : ControllerBase
    {
        private readonly GiteWritingService _writer;

        public PostGiteController(GiteWritingService writer)
        {
            _writer = writer;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GiteDto dto, CancellationToken ct)
        {
            var id = await _writer.CreateAsync(dto, ct);
            return Created($"/gite/get/{id}", null);
        }
    }
}

