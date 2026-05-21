using RunningEventsSystem.Application.Services;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.SharedKernel.Dto;
using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.Domain.Exceptions;
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
            EventDate = e.EventDate
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
            EventDate = eventEntity.EventDate
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