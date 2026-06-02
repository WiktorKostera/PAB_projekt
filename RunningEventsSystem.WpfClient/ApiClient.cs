using System.Net.Http;
using System.Net.Http.Json;
using RunningEventsSystem.SharedKernel.Dto;

namespace RunningEventsSystem.WpfClient;

public class ApiClient
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://localhost:44366";

    public ApiClient() { _http = new HttpClient(); }

    // EVENTS
    public Task<List<EventDto>?> GetEventsAsync()
        => _http.GetFromJsonAsync<List<EventDto>>($"{BaseUrl}/event");

    // USERS
    public Task<List<UserDto>?> GetUsersAsync()
        => _http.GetFromJsonAsync<List<UserDto>>($"{BaseUrl}/user");
    public Task<HttpResponseMessage> CreateUserAsync(CreateUserDto dto)
        => _http.PostAsJsonAsync($"{BaseUrl}/user", dto);

    // REGISTRATIONS
    public Task<List<RegistrationDto>?> GetRegistrationsByEventAsync(int eventId)
        => _http.GetFromJsonAsync<List<RegistrationDto>>($"{BaseUrl}/registration/event/{eventId}");
    public Task<List<RegistrationDto>?> GetRegistrationsByUserAsync(int userId)
        => _http.GetFromJsonAsync<List<RegistrationDto>>($"{BaseUrl}/registration/user/{userId}");
    public Task<HttpResponseMessage> CreateRegistrationAsync(CreateRegistrationDto dto)
        => _http.PostAsJsonAsync($"{BaseUrl}/registration", dto);
    public Task<HttpResponseMessage> CancelRegistrationAsync(int id)
        => _http.DeleteAsync($"{BaseUrl}/registration/{id}");
    public Task<HttpResponseMessage> PayRegistrationAsync(int id)
        => _http.PostAsync($"{BaseUrl}/registration/{id}/pay", null);

    // RESULTS
    public Task<List<ResultDto>?> GetResultsByEventAsync(int eventId)
        => _http.GetFromJsonAsync<List<ResultDto>>($"{BaseUrl}/result/event/{eventId}");
}
