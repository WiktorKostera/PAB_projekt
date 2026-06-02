using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RunningEventsSystem.BlazorWasm;
using RunningEventsSystem.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient pointing at our WebAPI
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7001/")
});

// Application services
builder.Services.AddScoped<IEventApiService, EventApiService>();
builder.Services.AddScoped<IUserApiService, UserApiService>();
builder.Services.AddScoped<IRegistrationApiService, RegistrationApiService>();
builder.Services.AddScoped<IResultApiService, ResultApiService>();
builder.Services.AddScoped<SessionService>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
