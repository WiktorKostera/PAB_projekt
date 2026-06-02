using Microsoft.AspNetCore.Mvc;
using RunningEventsSystem.Application.Services;
using RunningEventsSystem.SharedKernel.Dto;

namespace RunningEventsSystem.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    public RegistrationController(IRegistrationService registrationService)
    { _registrationService = registrationService; }

    [HttpGet("event/{eventId}")]
    public ActionResult<IEnumerable<RegistrationDto>> GetByEvent(int eventId)
        => Ok(_registrationService.GetByEvent(eventId));

    [HttpGet("user/{userId}")]
    public ActionResult<IEnumerable<RegistrationDto>> GetByUser(int userId)
        => Ok(_registrationService.GetByUser(userId));

    [HttpPost]
    public IActionResult Create([FromBody] CreateRegistrationDto dto)
    {
        var id = _registrationService.Create(dto);
        return Created($"/registration/{id}", null);
    }

    [HttpDelete("{id}")]
    public IActionResult Cancel(int id)
    {
        _registrationService.Cancel(id);
        return NoContent();
    }

    [HttpDelete("user/{userId}/event/{eventId}")]
    public IActionResult CancelByUserAndEvent(int userId, int eventId)
    {
        _registrationService.CancelByUserAndEvent(userId, eventId);
        return NoContent();
    }

    [HttpPost("{id}/pay")]
    public IActionResult Pay(int id)
    {
        _registrationService.Pay(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateRegistrationDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Id param is not valid");

        _registrationService.Update(dto);
        return NoContent();
    }
}
