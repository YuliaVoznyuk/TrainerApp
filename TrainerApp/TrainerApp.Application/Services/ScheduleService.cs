using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repo;

    public ScheduleService(IScheduleRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<object>> GetAllSlotsAsync() =>
        _repo.GetAllSlotsAsync();

    public Task<Guid> CreateSlotAsync(Guid trainerId, DateTime startAt, DateTime endAt, int maxClients, bool isOnline = false,
        string? link = null)
    {
       return _repo.CreateSlotAsync(trainerId, startAt, endAt, maxClients, isOnline, link);
    }

    public Task JoinSlotAsync(Guid clientId, Guid slotId)
    {
       return _repo.JoinSlotAsync(clientId, slotId);
    }

    public Task LeaveSlotAsync(Guid clientId, Guid slotId)
    {
       return _repo.LeaveSlotAsync(clientId, slotId);
    }

    public Task DeleteSlotAsync(Guid trainerId, Guid slotId)
    {
       return _repo.DeleteSlotAsync(trainerId, slotId);
    }
    public Task<Guid> GetTrainerIdAsync(ClaimsPrincipal userClaims)
    {
        var claim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (claim == null || !Guid.TryParse(claim, out var trainerId))
            throw new UnauthorizedAccessException("User is not a trainer.");
        return Task.FromResult(trainerId);
    }

    public Task<Guid> GetClientIdAsync(ClaimsPrincipal userClaims)
    {
        var claim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (claim == null || !Guid.TryParse(claim, out var clientId))
            throw new UnauthorizedAccessException("User is not a client.");
        return Task.FromResult(clientId);
    }
}