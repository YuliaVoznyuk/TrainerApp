namespace TrainerApp.Domain.Entities;

public class NutritionItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid NutritionPlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fats { get; set; }
    public string Description { get; set; } = null!;

    public NutritionPlan? NutritionPlan { get; set; }
}