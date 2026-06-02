using RunningEventsSystem.Application.Services;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Exceptions;
using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.SharedKernel.Dto;

public class UserService : IUserService
{
    private readonly IRunningEventsUnitOfWork _uow;
    public UserService(IRunningEventsUnitOfWork uow) { _uow = uow; }

    public List<UserDto> GetAll(string? search = null, string? sortBy = null, bool descending = false)
    {
        var query = _uow.UserRepository.GetAll().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var phrase = search.Trim().ToLower();
            query = query.Where(u =>
                u.FirstName.ToLower().Contains(phrase) ||
                u.LastName.ToLower().Contains(phrase) ||
                u.Email.ToLower().Contains(phrase));
        }

        query = (sortBy ?? "lastName").ToLower() switch
        {
            "firstname" => descending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
            "email" => descending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "role" => descending ? query.OrderByDescending(u => u.Role) : query.OrderBy(u => u.Role),
            _ => descending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName)
        };

        return query.Select(MapToDto).ToList();
    }

    public UserDto GetById(int id)
    {
        var u = _uow.UserRepository.Get(id) ?? throw new NotFoundException("User not found");
        return MapToDto(u);
    }

    public int Create(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            throw new BadRequestException("Email and password are required");

        if (_uow.UserRepository.GetByEmail(dto.Email) != null)
            throw new BadRequestException("Email already exists");

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email.Trim().ToLower(),
            Password = PasswordHasher.Hash(dto.Password),
            Role = UserRole.Participant,
            IsActive = true
        };
        _uow.UserRepository.Insert(user);
        _uow.Commit();
        return user.Id;
    }

    public void Update(UpdateUserDto dto)
    {
        var user = _uow.UserRepository.Get(dto.Id) ?? throw new NotFoundException("User not found");
        var emailOwner = _uow.UserRepository.GetByEmail(dto.Email);
        if (emailOwner != null && emailOwner.Id != dto.Id)
            throw new BadRequestException("Email already exists");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            throw new BadRequestException("Invalid user role");

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email.Trim().ToLower();
        user.Role = role;
        user.IsActive = dto.IsActive;
        _uow.Commit();
    }

    public void Delete(int id)
    {
        var user = _uow.UserRepository.Get(id) ?? throw new NotFoundException("User not found");
        user.IsActive = false;
        _uow.Commit();
    }

    public LoginResultDto Login(LoginDto dto)
    {
        var user = _uow.UserRepository.GetByEmail(dto.Email)
            ?? throw new BadRequestException("Invalid email or password");

        if (!user.IsActive || !PasswordHasher.Verify(dto.Password, user.Password))
            throw new BadRequestException("Invalid email or password");

        return new LoginResultDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        IsActive = user.IsActive,
        Role = user.Role.ToString()
    };
}
