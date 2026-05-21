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

builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CreateEventDtoValidator>();
});

builder.Services.AddDbContext<RunningEventsDbContext>(options =>
    options.UseSqlite("Data Source=runningevents.db"));

builder.Services.AddScoped<IValidator<CreateEventDto>, CreateEventDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateEventDto>, UpdateEventDtoValidator>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();

builder.Services.AddScoped<IRunningEventsUnitOfWork, RunningEventsUnitOfWork>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<ExceptionMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    dataSeeder.Seed();
}

app.Run();
