using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("reservation/patch/[Controller]")]
public sealed class UpdateReservationController : ControllerBase
{
    private readonly IReservationCommandService _commands;

    public UpdateReservationController(IReservationCommandService commands)
    {
        _commands = commands;
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchReservationRequestDto request, CancellationToken ct)
    {
        if (request.ReservationId != id)
            return BadRequest(new { message = "Route id does not match body ReservationId." });

        var updated = await _commands.PatchAsync(request, ct);
        if (updated is null) return NotFound();

        return Ok(updated.ReservationId);
    }
}
