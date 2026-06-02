namespace RunningEventsSystem.SharedKernel.Dto;

public class ResultDto
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public int EventId { get; set; }
	public int RegistrationId { get; set; }
	public string UserFullName { get; set; } = string.Empty;
	public int Place { get; set; }
	public TimeSpan FinishTime { get; set; }
}

public class CreateResultDto
{
	public int RegistrationId { get; set; }
	public int UserId { get; set; }
	public int EventId { get; set; }
	public TimeSpan FinishTime { get; set; }
	public int Place { get; set; }
}

public class UpdateResultDto
{
	public int Id { get; set; }
	public TimeSpan FinishTime { get; set; }
	public int Place { get; set; }
}
