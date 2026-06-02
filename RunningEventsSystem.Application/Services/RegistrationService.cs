using RunningEventsSystem.Application.Services;
using RunningEventsSystem.Domain.Contracts;
using RunningEventsSystem.Domain.Exceptions;
using RunningEventsSystem.Domain.Models;
using RunningEventsSystem.SharedKernel.Dto;

public class RegistrationService : IRegistrationService
{
	private readonly IRunningEventsUnitOfWork _uow;
	public RegistrationService(IRunningEventsUnitOfWork uow) { _uow = uow; }

	public List<RegistrationDto> GetAll() =>
		_uow.RegistrationRepository
			.GetAll()
			.Select(MapToDto).ToList();

	public RegistrationDto GetById(int id)
	{
		var reg = _uow.RegistrationRepository.Get(id) ?? throw new NotFoundException("Registration not found");
		return MapToDto(reg);
	}

	public List<RegistrationDto> GetByEvent(int eventId) =>
		_uow.RegistrationRepository
			.GetByEventWithDetails(eventId)
			.Select(MapToDto).ToList();

	public List<RegistrationDto> GetByUser(int userId) =>
		_uow.RegistrationRepository
			.GetByUserWithDetails(userId)
			.Select(MapToDto).ToList();

	public int Create(CreateRegistrationDto dto)
	{
		var ev = _uow.EventRepository.Get(dto.EventId) ?? throw new NotFoundException("Event not found");
		_ = _uow.UserRepository.Get(dto.UserId) ?? throw new NotFoundException("User not found");

		if (ev.RegistrationDeadline < DateTime.Now)
			throw new BadRequestException("Registration deadline has passed");

		if (_uow.RegistrationRepository.IsUserRegistered(dto.UserId, dto.EventId))
			throw new BadRequestException("User already registered for this event");

		var currentCount = _uow.RegistrationRepository.Find(r => r.EventId == dto.EventId && r.Status != RegistrationStatus.Cancelled).Count;
		if (currentCount >= ev.MaxParticipants)
			throw new BadRequestException("Event is full");

		var reg = new Registration
		{
			UserId = dto.UserId,
			EventId = dto.EventId,
			RegistrationDate = DateTime.UtcNow,
			Status = RegistrationStatus.Confirmed,
			PaymentStatus = PaymentStatus.Unpaid,
			PaymentAmount = ev.EntryFee
		};
		_uow.RegistrationRepository.Insert(reg);
		_uow.Commit();
		return reg.Id;
	}

	public void Cancel(int id)
	{
		var reg = _uow.RegistrationRepository.Get(id) ?? throw new NotFoundException("Registration not found");
		reg.Status = RegistrationStatus.Cancelled;
		reg.CancelledAt = DateTime.UtcNow;
		_uow.Commit();
	}

	public void CancelByUserAndEvent(int userId, int eventId)
	{
		var reg = _uow.RegistrationRepository.GetActiveByUserAndEvent(userId, eventId)
			?? throw new NotFoundException("Registration not found");

		reg.Status = RegistrationStatus.Cancelled;
		reg.CancelledAt = DateTime.UtcNow;
		_uow.Commit();
	}

	public void Pay(int id)
	{
		var reg = _uow.RegistrationRepository.Get(id) ?? throw new NotFoundException("Registration not found");
		if (reg.Status == RegistrationStatus.Cancelled)
			throw new BadRequestException("Cancelled registration cannot be paid");

		reg.PaymentStatus = PaymentStatus.Paid;
		reg.PaidAt = DateTime.UtcNow;
		_uow.Commit();
	}

	public void Update(UpdateRegistrationDto dto)
	{
		var reg = _uow.RegistrationRepository.Get(dto.Id) ?? throw new NotFoundException("Registration not found");
		if (!Enum.TryParse<RegistrationStatus>(dto.Status, true, out var status))
			throw new BadRequestException("Invalid registration status");

		reg.Status = status;
		reg.StartNumber = dto.StartNumber;
		reg.CancelledAt = status == RegistrationStatus.Cancelled ? DateTime.UtcNow : null;
		_uow.Commit();
	}

	private static RegistrationDto MapToDto(Registration r) => new()
	{
		Id = r.Id,
		UserId = r.UserId,
		UserFullName = $"{r.User?.FirstName} {r.User?.LastName}",
		EventId = r.EventId,
		EventName = r.Event?.Name ?? "",
		RegistrationDate = r.RegistrationDate,
		Status = r.Status.ToString(),
		PaymentStatus = r.PaymentStatus.ToString(),
		PaidAt = r.PaidAt,
		PaymentAmount = r.PaymentAmount,
		CancelledAt = r.CancelledAt,
		StartNumber = r.StartNumber
	};
}
