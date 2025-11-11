namespace TrainerApp.Application.DTOs;

public class WorkoutSessionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ExerciseRecordDto> Exercises { get; set; } = new List<ExerciseRecordDto>();
    public Guid ClientId { get; set; }
    public DateTime Date { get; set; }
}