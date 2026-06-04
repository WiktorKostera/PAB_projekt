using RunningEventsSystem.SharedKernel.Dto;
using System.Net.Http.Json;

namespace RunningEventsSystem.BlazorServer.Services;

public interface IAdminApiService
{
    // Events
    Task<List<EventDto>> GetEventsAsync();
    Task<EventDto?> GetEventByIdAsync(int id);
    Task CreateEventAsync(CreateEventDto dto);
    Task UpdateEventAsync(int id, UpdateEventDto dto);
    Task DeleteEventAsync(int id);

    // Users
    Task<List<UserDto>> GetUsersAsync();
    Task UpdateUserAsync(int id, UpdateUserDto dto);
    Task DeleteUserAsync(int id);

    // Registrations
    Task<List<RegistrationDto>> GetRegistrationsByEventAsync(int eventId);
    Task<List<RegistrationDto>> GetRegistrationsByUserAsync(int userId);
    Task UpdateRegistrationAsync(int id, UpdateRegistrationDto dto);

    // Results
    Task<List<ResultDto>> GetResultsByEventAsync(int eventId);
    Task CreateResultAsync(CreateResultDto dto);
}

public class AdminApiService : IAdminApiService
{
    private readonly HttpClient _http;
    private readonly ILogger<AdminApiService> _logger;

    public AdminApiService(HttpClient http, ILogger<AdminApiService> logger)
    {
        _http = http;
        _logger = logger;
    }

    // Events
    public async Task<List<EventDto>> GetEventsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<EventDto>>("event") ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd pobierania listy eventów");
            return new();
        }
    }

    public async Task<EventDto?> GetEventByIdAsync(int id)
    {
        try { return await _http.GetFromJsonAsync<EventDto>($"event/{id}"); }
        catch (Exception ex) { _logger.LogError(ex, "Błąd pobierania eventu {Id}", id); return null; }
    }

    public async Task CreateEventAsync(CreateEventDto dto)
    {
        var r = await _http.PostAsJsonAsync("event", dto);
        r.EnsureSuccessStatusCode();
        if (!r.IsSuccessStatusCode)
            _logger.LogError("Błąd tworzenia eventu: {Status}", r.StatusCode);
    }

    public async Task UpdateEventAsync(int id, UpdateEventDto dto)
    {
        var r = await _http.PutAsJsonAsync($"event/{id}", dto);
        if (!r.IsSuccessStatusCode)
            _logger.LogError("Błąd aktualizacji eventu {Id}: {Status}", id, r.StatusCode);
    }

    public async Task DeleteEventAsync(int id)
    {
        var r = await _http.DeleteAsync($"event/{id}");
        if (!r.IsSuccessStatusCode)
            _logger.LogError("Błąd usuwania eventu {Id}: {Status}", id, r.StatusCode);
    }

    // Users
    public async Task<List<UserDto>> GetUsersAsync()
    {
        try { return await _http.GetFromJsonAsync<List<UserDto>>("user") ?? new(); }
        catch (Exception ex) { _logger.LogError(ex, "Błąd pobierania listy użytkowników"); return new(); }
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var r = await _http.PutAsJsonAsync($"user/{id}", dto);
        if (!r.IsSuccessStatusCode)
            _logger.LogError("Błąd aktualizacji użytkownika {Id}", id);
    }

    public async Task DeleteUserAsync(int id)
    {
        var r = await _http.DeleteAsync($"user/{id}");
        if (!r.IsSuccessStatusCode)
            _logger.LogError("Błąd usuwania użytkownika {Id}", id);
    }

    // Registrations
    public async Task<List<RegistrationDto>> GetRegistrationsByEventAsync(int eventId)
    {
        try { return await _http.GetFromJsonAsync<List<RegistrationDto>>($"registration/event/{eventId}") ?? new(); }
        catch (Exception ex) { _logger.LogError(ex, "Błąd pobierania zapisów dla eventu {Id}", eventId); return new(); }
    }

    public async Task<List<RegistrationDto>> GetRegistrationsByUserAsync(int userId)
    {
        try { return await _http.GetFromJsonAsync<List<RegistrationDto>>($"registration/user/{userId}") ?? new(); }
        catch (Exception ex) { _logger.LogError(ex, "Błąd pobierania zapisów użytkownika {Id}", userId); return new(); }
    }

    public async Task UpdateRegistrationAsync(int id, UpdateRegistrationDto dto)
    {
        var r = await _http.PutAsJsonAsync($"registration/{id}", dto);
        if (!r.IsSuccessStatusCode)
            _logger.LogError("Błąd aktualizacji zapisu {Id}", id);
    }

    // Results
    public async Task<List<ResultDto>> GetResultsByEventAsync(int eventId)
    {
        try { return await _http.GetFromJsonAsync<List<ResultDto>>($"result/event/{eventId}") ?? new(); }
        catch (Exception ex) { _logger.LogError(ex, "Błąd pobierania wyników dla eventu {Id}", eventId); return new(); }
    }

    public async Task CreateResultAsync(CreateResultDto dto)
    {
        var r = await _http.PostAsJsonAsync("result", dto);
        if (!r.IsSuccessStatusCode)
            _logger.LogError("Błąd tworzenia wyniku");
    }
}
