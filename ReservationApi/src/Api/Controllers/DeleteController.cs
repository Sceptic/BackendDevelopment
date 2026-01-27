using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("reservation/delete/[controller]")]
public sealed class DeleteReservationController : ControllerBase
{
    private readonly IReservationReadService _read;
    private readonly IReservationCommandService _commands;

    public DeleteReservationController(IReservationReadService read, IReservationCommandService commands)
    {
        _read = read;
        _commands = commands;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _commands.DeleteAsync(id, ct);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
