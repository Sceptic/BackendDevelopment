using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Read;

namespace Api.Controllers;

[ApiController]
[Route("reservation/post/[controller]")]
public sealed class CreateReservationController : ControllerBase
{
    private readonly IReservationReadService _read;
    private readonly IReservationCommandService _commands;

    public CreateReservationController(IReservationReadService read, IReservationCommandService commands)
    {
        _read = read;
        _commands = commands;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequestDto request, CancellationToken ct)
    {
        var id = await _commands.CreateAsync(request, ct);
        return Created($"/reservation/get/{id}", null);
    }
}
