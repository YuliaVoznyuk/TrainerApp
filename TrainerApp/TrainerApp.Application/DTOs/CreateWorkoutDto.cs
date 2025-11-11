namespace TrainerApp.Application.DTOs;

public class CreateWorkoutDto
{
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Guid ClientId { get; set; }
    public List<CreateExerciseDto> Exercises { get; set; } = new();
}