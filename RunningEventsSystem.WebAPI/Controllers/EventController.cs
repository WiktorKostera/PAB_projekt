using Microsoft.AspNetCore.Mvc;
using RunningEventsSystem.Application.Services;
using RunningEventsSystem.SharedKernel.Dto;

namespace RunningEventsSystem.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<EventDto>> GetAll()
    {
        var result = _eventService.GetAll();
        _logger.LogDebug("Pobrano listę eventów");
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EventDto> GetById(int id)
    {
        var result = _eventService.GetById(id);
        _logger.LogDebug($"Pobrano event o id = {id}");
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateEventDto dto)
    {
        var id = _eventService.Create(dto);
        _logger.LogDebug($"Utworzono event o id = {id}");
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] UpdateEventDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Id param is not valid");

        _eventService.Update(dto);
        _logger.LogDebug($"Zaktualizowano event o id = {id}");
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        _eventService.Delete(id);
        _logger.LogDebug($"Usunięto event o id = {id}");
        return NoContent();
    }
}