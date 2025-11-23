using System.Security.Claims;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Interfaces;

public interface ITrainerService
{
    Task<Guid> GetTrainerIdAsync(ClaimsPrincipal userClaims);

    Task<IEnumerable<ClientDto>> GetClientsAsync(Guid trainerId);
    Task<IEnumerable<NutritionPlanDto>> GetNutritionPlansAsync(Guid trainerId);
    Task<IEnumerable<WorkoutSessionDto>> GetWorkoutSessionsAsync(Guid trainerId);
    Task AddOrUpdateNutritionPlanAsync(Guid trainerId, NutritionPlanDto dto);
}