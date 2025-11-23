using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScheduleSlot>> GetClientTrainingsAsync(Guid clientId)
    {
        return await _context.ScheduleSlots
            .Include(s => s.Trainer)
            .Include(s => s.Clients)
            .Where(s => s.Clients.Any(c => c.Id == clientId))
            .OrderBy(s => s.StartAt)
            .ToListAsync();
    }

    public async Task<NutritionPlan?> GetClientNutritionPlanAsync(Guid clientId)
    {
        return await _context.NutritionPlans
            .FirstOrDefaultAsync(p => p.ClientId == clientId);
    }

    public async Task<ScheduleSlot?> GetSlotByIdAsync(Guid slotId)
    {
        return await _context.ScheduleSlots
            .Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == slotId);
    }

    public async Task<Client?> GetClientByIdAsync(Guid clientId)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId);
    }

    public async Task AddProgressPhotoAsync(Photo photo)
    {
        await _context.Photos.AddAsync(photo);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}