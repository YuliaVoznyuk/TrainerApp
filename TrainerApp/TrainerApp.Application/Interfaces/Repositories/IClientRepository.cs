using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Interfaces.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<ScheduleSlot>> GetClientTrainingsAsync(Guid clientId);
    Task<NutritionPlan?> GetClientNutritionPlanAsync(Guid clientId);
    Task<ScheduleSlot?> GetSlotByIdAsync(Guid slotId);
    Task<Client?> GetClientByIdAsync(Guid clientId);
    Task AddProgressPhotoAsync(Photo photo);
    Task SaveChangesAsync();
}