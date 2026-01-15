using Application.Gites.ReadQueries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("gite/get/")]
public class GetGiteController : ControllerBase
{
    private readonly GiteReadingService _readingService;

    public GetGiteController(GiteReadingService giteService)
    {
        _readingService = giteService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var gites = await _readingService.GetAllAsync(ct);
        return Ok(gites);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var gite = await _readingService.GetByIdAsync(id, ct);
        if (gite is null) return NotFound();
        return Ok(gite);
    }
}
