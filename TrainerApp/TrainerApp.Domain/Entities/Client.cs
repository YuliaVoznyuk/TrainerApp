namespace TrainerApp.Domain.Entities;

public class Client: User
{
    public double? HeightCm { get; set; }
    public double? CurrentWeightKg { get; set; }
    public Guid? TrainerId { get; set; }
    public Trainer Trainer { get; set; } = null!;
    public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
    public ICollection<NutritionPlan> NutritionPlans { get; set; } = new List<NutritionPlan>();
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<ScheduleSlot> ScheduleSlots { get; set; } = new List<ScheduleSlot>();

}