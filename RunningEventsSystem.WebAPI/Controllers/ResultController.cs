using Microsoft.AspNetCore.Mvc;
using RunningEventsSystem.Application.Services;
using RunningEventsSystem.SharedKernel.Dto;

namespace RunningEventsSystem.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ResultController : ControllerBase
{
    private readonly IResultService _resultService;
    public ResultController(IResultService resultService) { _resultService = resultService; }

    [HttpGet("event/{eventId}")]
    public ActionResult<IEnumerable<ResultDto>> GetByEvent(int eventId)
        => Ok(_resultService.GetByEvent(eventId));

    [HttpPost]
    public IActionResult Create([FromBody] CreateResultDto dto)
    {
        var id = _resultService.Create(dto);
        return Created($"/result/{id}", null);
    }
}