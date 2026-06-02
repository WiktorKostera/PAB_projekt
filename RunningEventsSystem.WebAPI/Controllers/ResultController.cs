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

    [HttpGet]
    public ActionResult<IEnumerable<ResultDto>> GetAll()
        => Ok(_resultService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<ResultDto> GetById(int id)
        => Ok(_resultService.GetById(id));

    [HttpGet("event/{eventId}")]
    public ActionResult<IEnumerable<ResultDto>> GetByEvent(int eventId)
        => Ok(_resultService.GetByEvent(eventId));

    [HttpPost]
    public IActionResult Create([FromBody] CreateResultDto dto)
    {
        var id = _resultService.Create(dto);
        return Created($"/result/{id}", null);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateResultDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Id param is not valid");

        _resultService.Update(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _resultService.Delete(id);
        return NoContent();
    }
}
