namespace TrainerApp.Application.DTOs;

public class CreateNutritionPlanDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid ClientId { get; set; }
    public List<CreateNutritionItemDto> Items { get; set; } = new();
}