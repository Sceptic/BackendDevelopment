using Api.DtoModels;
using Application.Gites.WriteQueries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("gite/put")]
    public sealed class PutGitesController : ControllerBase
    {
        private readonly GiteWritingService _writer;

        public PutGitesController(GiteWritingService writer)
        {
            _writer = writer;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] GiteDto dto, CancellationToken ct)
        {
            try
            {
                await _writer.UpdateAsync(id, dto, ct);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
