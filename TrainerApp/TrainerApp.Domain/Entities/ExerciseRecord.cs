namespace TrainerApp.Domain.Entities;

public class ExerciseRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WorkoutSessionId { get; set; }
    public string Name { get; set; } = null!;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double WeightKg { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? Notes { get; set; }

    public WorkoutSession? WorkoutSession { get; set; }
}