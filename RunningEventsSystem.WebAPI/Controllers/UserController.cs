using Microsoft.AspNetCore.Mvc;
using RunningEventsSystem.Application.Services;
using RunningEventsSystem.SharedKernel.Dto;

namespace RunningEventsSystem.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) { _userService = userService; }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll() => Ok(_userService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<UserDto> GetById(int id) => Ok(_userService.GetById(id));

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserDto dto)
    {
        var id = _userService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPost("login")]
    public ActionResult<LoginResultDto> Login([FromBody] LoginDto dto)
    {
        return Ok(_userService.Login(dto));
    }
}
