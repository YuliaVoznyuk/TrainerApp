namespace TrainerApp.Domain.Entities;

public class NutritionPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid TrainerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }

    public ICollection<NutritionItem> Items { get; set; } = new List<NutritionItem>();

    public Client? Client { get; set; }
    public Trainer? Trainer { get; set; }
}