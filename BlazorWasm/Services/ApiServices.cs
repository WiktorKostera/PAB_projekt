using RunningEventsSystem.SharedKernel.Dto;
using System.Net.Http.Json;

namespace RunningEventsSystem.BlazorWasm.Services;

// ── Events ──────────────────────────────────────────────────────────────────

public interface IEventApiService
{
    Task<List<EventDto>> GetAllAsync();
    Task<EventDto?> GetByIdAsync(int id);
}

public class EventApiService : IEventApiService
{
    private readonly HttpClient _http;
    public EventApiService(HttpClient http) => _http = http;

    public async Task<List<EventDto>> GetAllAsync()
        => await _http.GetFromJsonAsync<List<EventDto>>("event") ?? new();

    public async Task<EventDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<EventDto>($"event/{id}");
}

// ── Users ────────────────────────────────────────────────────────────────────

public interface IUserApiService
{
    Task<LoginResultDto?> LoginAsync(LoginDto dto);
    Task<int> RegisterAsync(CreateUserDto dto);
    Task<UserDto?> GetByIdAsync(int id);
}

public class UserApiService : IUserApiService
{
    private readonly HttpClient _http;
    public UserApiService(HttpClient http) => _http = http;

    public async Task<LoginResultDto?> LoginAsync(LoginDto dto)
    {
        var response = await _http.PostAsJsonAsync("user/login", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<LoginResultDto>();
    }

    public async Task<int> RegisterAsync(CreateUserDto dto)
    {
        var response = await _http.PostAsJsonAsync("user", dto);
        response.EnsureSuccessStatusCode();
        // Created returns 201 with Location header; we get id from location
        var location = response.Headers.Location?.ToString() ?? "";
        if (int.TryParse(location.Split('/').LastOrDefault(), out var id)) return id;
        return 0;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<UserDto>($"user/{id}");
}

// ── Registrations ────────────────────────────────────────────────────────────

public interface IRegistrationApiService
{
    Task<List<RegistrationDto>> GetByUserAsync(int userId);
    Task<List<RegistrationDto>> GetByEventAsync(int eventId);
    Task<HttpResponseMessage> CreateAsync(CreateRegistrationDto dto);
    Task CancelByUserAndEventAsync(int userId, int eventId);
    Task PayAsync(int registrationId);
}

public class RegistrationApiService : IRegistrationApiService
{
    private readonly HttpClient _http;
    public RegistrationApiService(HttpClient http) => _http = http;

    public async Task<List<RegistrationDto>> GetByUserAsync(int userId)
        => await _http.GetFromJsonAsync<List<RegistrationDto>>($"registration/user/{userId}") ?? new();

    public async Task<List<RegistrationDto>> GetByEventAsync(int eventId)
        => await _http.GetFromJsonAsync<List<RegistrationDto>>($"registration/event/{eventId}") ?? new();

    public Task<HttpResponseMessage> CreateAsync(CreateRegistrationDto dto)
        => _http.PostAsJsonAsync("registration", dto);

    public Task<HttpResponseMessage> CancelByUserAndEventAsync(int userId, int eventId)
        => _http.DeleteAsync($"registration/user/{userId}/event/{eventId}");

    public Task<HttpResponseMessage> PayAsync(int registrationId)
        => _http.PostAsync($"registration/{registrationId}/pay", null);
}

// ── Results ──────────────────────────────────────────────────────────────────

public interface IResultApiService
{
    Task<List<ResultDto>> GetByEventAsync(int eventId);
}

public class ResultApiService : IResultApiService
{
    private readonly HttpClient _http;
    public ResultApiService(HttpClient http) => _http = http;

    public async Task<List<ResultDto>> GetByEventAsync(int eventId)
        => await _http.GetFromJsonAsync<List<ResultDto>>($"result/event/{eventId}") ?? new();
}
