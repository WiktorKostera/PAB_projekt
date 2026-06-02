using RunningEventsSystem.SharedKernel.Dto;
namespace RunningEventsSystem.Application.Services;

public interface IUserService
{
    List<UserDto> GetAll(string? search = null, string? sortBy = null, bool descending = false);
    UserDto GetById(int id);
    int Create(CreateUserDto dto);
    void Update(UpdateUserDto dto);
    void Delete(int id);
    LoginResultDto Login(LoginDto dto);
}
