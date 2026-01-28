using System.Security.Claims;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Interfaces;

public interface INutritionService
{
    Task<IEnumerable<NutritionPlanDto>> GetTrainerPlansAsync();
    Task<NutritionPlanDto?> GetByIdAsync(Guid planId);
    Task<Guid> CreateAsync(CreateNutritionPlanDto dto);
    Task UpdateAsync(Guid planId, CreateNutritionPlanDto dto);
    Task DeleteAsync(Guid planId);
}