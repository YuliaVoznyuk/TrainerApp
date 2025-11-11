namespace TrainerApp.Domain.Entities;

public class WorkoutSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid TrainerId { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }

    public ICollection<ExerciseRecord> ExerciseRecords { get; set; } = new List<ExerciseRecord>();

    public Client? Client { get; set; }
    public Trainer? Trainer { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
}