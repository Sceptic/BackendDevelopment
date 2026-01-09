using Application.Hotelrooms.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("hotelroom")]
public class HotelroomController : ControllerBase
{
    private readonly GetAllHotelroomsQuery _getAll;
    private readonly GetHotelroomByIdQuery _getById;

    public HotelroomController(GetAllHotelroomsQuery getAll, GetHotelroomByIdQuery getById)
    {
        _getAll = getAll;
        _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _getAll.ExecuteAsync();
        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var room = await _getById.ExecuteAsync(id);
        if (room is null) return NotFound();
        return Ok(room);
    }
}
