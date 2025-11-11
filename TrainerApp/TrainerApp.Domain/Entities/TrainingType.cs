namespace TrainerApp.Domain.Entities;

public class TrainingType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}