using Application.Gites.WriteQueries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("gite/delete")]
    public sealed class DeleteGiteController : ControllerBase
    {
        private readonly GiteWritingService _writer;

        public DeleteGiteController(GiteWritingService writer)
        {
            _writer = writer;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                await _writer.DeleteAsync(id, ct);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
