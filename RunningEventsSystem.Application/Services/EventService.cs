using Microsoft.Extensions.Logging;
using RunningEventsSystem.Application.Services;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Exceptions;
using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.SharedKernel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventService : IEventService
{
    private readonly IRunningEventsUnitOfWork _uow;

    public EventService(IRunningEventsUnitOfWork uow)
    {
        _uow = uow;
    }

    public List<EventDto> GetAll()
    {
        var events = _uow.EventRepository.GetAll();

        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Name = e.Name,
            City = e.City,
            Location = e.Location,
            Description = e.Description,
            EventDate = e.EventDate,
            RegistrationDeadline = e.RegistrationDeadline,
            MaxParticipants = e.MaxParticipants,
            EntryFee = e.EntryFee,
            ImageUrl = e.ImageUrl,
            IsActive = e.IsActive
        }).ToList();
    }

    public EventDto GetById(int id)
    {
        var eventEntity = _uow.EventRepository.Get(id);

        if (eventEntity == null)
            throw new NotFoundException("Event not found");

        return new EventDto
        {
            Id = eventEntity.Id,
            Name = eventEntity.Name,
            City = eventEntity.City,
            Location = eventEntity.Location,
            Description = eventEntity.Description,
            EventDate = eventEntity.EventDate,
            RegistrationDeadline = eventEntity.RegistrationDeadline,
            MaxParticipants = eventEntity.MaxParticipants,
            EntryFee = eventEntity.EntryFee,
            ImageUrl = eventEntity.ImageUrl,
            IsActive = eventEntity.IsActive
        };
    }

    public int Create(CreateEventDto dto)
    {
        if (dto == null)
            throw new BadRequestException("Event data is null");

        var eventEntity = new Event
        {
            Name = dto.Name,
            Description = dto.Description,
            Location = dto.Location,
            City = dto.City,
            EventDate = dto.EventDate,
            RegistrationDeadline = dto.RegistrationDeadline,
            MaxParticipants = dto.MaxParticipants,
            EntryFee = dto.EntryFee,

            ImageUrl = string.IsNullOrEmpty(dto.ImageUrl)
                ? "/images/default-event.png"
                : dto.ImageUrl,

            IsActive = true
        };

        _uow.EventRepository.Insert(eventEntity);

        _uow.Commit();

        return eventEntity.Id;
    }

    public void Delete(int id)
    {
        var eventEntity = _uow.EventRepository.Get(id);

        if (eventEntity == null)
            throw new NotFoundException("Event not found");

        _uow.EventRepository.Delete(eventEntity);

        _uow.Commit();
    }
    public void Update(UpdateEventDto dto)
    {
        var eventEntity = _uow.EventRepository.Get(dto.Id);

        if (eventEntity == null)
            throw new NotFoundException("Event not found");

        eventEntity.Name = dto.Name;
        eventEntity.Description = dto.Description;
        eventEntity.Location = dto.Location;
        eventEntity.City = dto.City;
        eventEntity.EventDate = dto.EventDate;
        eventEntity.RegistrationDeadline = dto.RegistrationDeadline;
        eventEntity.MaxParticipants = dto.MaxParticipants;
        eventEntity.EntryFee = dto.EntryFee;

        eventEntity.ImageUrl = string.IsNullOrEmpty(dto.ImageUrl)
            ? "/images/default-event.png"
            : dto.ImageUrl;

        _uow.Commit();
    }
}
