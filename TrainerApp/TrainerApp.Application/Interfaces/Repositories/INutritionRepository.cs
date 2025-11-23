using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Interfaces.Repositories;

public interface INutritionRepository
{
    Task<IEnumerable<NutritionPlan>> GetTrainerPlansAsync(Guid trainerId);
    Task<NutritionPlan?> GetPlanByIdAsync(Guid trainerId, Guid planId);
    Task<Guid> CreatePlanAsync(NutritionPlan plan);
    Task UpdatePlanAsync(NutritionPlan plan);
    Task DeletePlanAsync(Guid trainerId, Guid planId);
    Task SaveChangesAsync();

}