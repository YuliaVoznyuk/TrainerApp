namespace TrainerApp.Application.Interfaces.Repositories;

public interface IScheduleRepository
{
    Task<IEnumerable<object>> GetAllSlotsAsync();
    Task<Guid> CreateSlotAsync(Guid trainerId, DateTime startAt, DateTime endAt, int maxClients, bool isOnline, string? link);
    Task JoinSlotAsync(Guid clientId, Guid slotId);
    Task LeaveSlotAsync(Guid clientId, Guid slotId);
    Task DeleteSlotAsync(Guid trainerId, Guid slotId);
}