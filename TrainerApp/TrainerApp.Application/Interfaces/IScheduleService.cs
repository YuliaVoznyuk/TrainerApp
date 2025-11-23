using System.Security.Claims;

namespace TrainerApp.Application.Interfaces;

public interface IScheduleService
{
    Task<IEnumerable<object>> GetAllSlotsAsync();
    Task<Guid> CreateSlotAsync(Guid trainerId, DateTime startAt, DateTime endAt, int maxClients, bool isOnline = false, string? link = null);
    Task JoinSlotAsync(Guid clientId, Guid slotId);
    Task LeaveSlotAsync(Guid clientId, Guid slotId);
    Task DeleteSlotAsync(Guid trainerId, Guid slotId);
    Task<Guid> GetTrainerIdAsync(ClaimsPrincipal userClaims);
    Task<Guid> GetClientIdAsync(ClaimsPrincipal userClaims);
}