using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RunningEventsSystem.Application.Services;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Infrastructure;
using RunningEventsSystem.Infrastructure.Repositories;
using RunningEventsSystem.SharedKernel.Dto;
using RunningEventsSystem.WebAPI.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();

builder.Services.AddDbContext<RunningEventsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IValidator<CreateEventDto>, CreateEventDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateEventDto>, UpdateEventDtoValidator>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();

builder.Services.AddScoped<IRunningEventsUnitOfWork, RunningEventsUnitOfWork>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<ExceptionMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    dataSeeder.Seed();
}

app.Run();
