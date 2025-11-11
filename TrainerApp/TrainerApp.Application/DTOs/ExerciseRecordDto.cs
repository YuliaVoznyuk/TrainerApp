namespace TrainerApp.Application.DTOs;

public class ExerciseRecordDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
}