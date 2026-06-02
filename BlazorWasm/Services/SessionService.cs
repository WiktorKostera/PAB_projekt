using RunningEventsSystem.SharedKernel.Dto;

namespace RunningEventsSystem.BlazorWasm.Services;

public class SessionService
{
    public LoginResultDto? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;
    public bool IsAdmin => CurrentUser?.Role == "Admin";

    public event Action? OnChange;

    public void Login(LoginResultDto user)
    {
        CurrentUser = user;
        OnChange?.Invoke();
    }

    public void Logout()
    {
        CurrentUser = null;
        OnChange?.Invoke();
    }
}
