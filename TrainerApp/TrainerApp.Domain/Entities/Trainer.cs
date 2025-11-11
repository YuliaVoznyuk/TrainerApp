namespace TrainerApp.Domain.Entities;

public class Trainer: User
{
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public ICollection<TrainingType> TrainingTypes { get; set; } = new List<TrainingType>();
    public ICollection<ScheduleSlot> ScheduleSlots { get; set; } = new List<ScheduleSlot>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<NutritionPlan> NutritionPlans { get; set; } = new List<NutritionPlan>();
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}