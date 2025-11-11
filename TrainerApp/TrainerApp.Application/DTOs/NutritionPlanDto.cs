namespace TrainerApp.Application.DTOs;

public class NutritionPlanDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid ClientId { get; set; }
    public IEnumerable<NutritionItemDto> Items { get; set; }
}