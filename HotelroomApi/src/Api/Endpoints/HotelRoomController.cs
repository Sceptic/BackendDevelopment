using Application.Abstractions.Persistence;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("hotelroom")]
public class HotelroomController : ControllerBase
{
    private readonly IHotelroomRepository _repository;

    public HotelroomController(IHotelroomRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _repository.GetAllAsync();
        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var room = await _repository.GetByIdAsync(id);
        if (room is null) return NotFound();
        return Ok(room);
    }
}
