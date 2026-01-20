using System.Security.Claims;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Interfaces;

public interface INutritionService
{
    Task<IEnumerable<NutritionPlanDto>> GetTrainerPlansAsync(Guid trainerId);
    Task<NutritionPlanDto?> GetByIdAsync(Guid trainerId, Guid planId);
    Task<Guid> CreateAsync(Guid trainerId, CreateNutritionPlanDto dto);
    Task UpdateAsync(Guid trainerId, Guid planId, CreateNutritionPlanDto dto);
    Task DeleteAsync(Guid trainerId, Guid planId);
}