using Application.Abstractions.Reservations;
using Application.DTOs.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Read;

[ApiController]
[Route("reservation/get/[controller]")]
public sealed class ReadReservationsController : ControllerBase
{
    private readonly IReservationReadService _read;
    private readonly IReservationCommandService _commands;

    public ReadReservationsController(IReservationReadService read, IReservationCommandService commands)
    {
        _read = read;
        _commands = commands;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _read.GetAllAsync(ct);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await _read.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        return Ok(item);
    }  
}
