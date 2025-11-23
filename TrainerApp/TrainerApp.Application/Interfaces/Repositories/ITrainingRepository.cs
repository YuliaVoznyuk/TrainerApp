using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Interfaces.Repositories;

public interface ITrainingRepository
{
    Task<IEnumerable<ScheduleSlot>> GetClientTrainingsAsync(Guid clientId);
    Task<ScheduleSlot?> GetSlotByIdAsync(Guid slotId);
    Task RemoveClientFromSlotAsync(Guid clientId, Guid slotId);
    Task<Trainer?> GetTrainerWithClientsAsync(Guid trainerId);
    Task<IEnumerable<NutritionPlan>> GetNutritionPlansAsync(Guid trainerId);
    Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsAsync(Guid trainerId);
    Task<NutritionPlan?> GetNutritionPlanByIdAsync(Guid planId);
    Task AddNutritionPlanAsync(NutritionPlan plan);
    Task UpdateNutritionPlanAsync(NutritionPlan plan);
}