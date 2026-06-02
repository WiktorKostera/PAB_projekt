namespace RunningEventsSystem.SharedKernel.Dto;

public class RegistrationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime? PaidAt { get; set; }
    public decimal PaymentAmount { get; set; }
    public DateTime? CancelledAt { get; set; }
    public int? StartNumber { get; set; }
}

public class CreateRegistrationDto
{
    public int UserId { get; set; }
    public int EventId { get; set; }
}

public class UpdateRegistrationDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? StartNumber { get; set; }
}
