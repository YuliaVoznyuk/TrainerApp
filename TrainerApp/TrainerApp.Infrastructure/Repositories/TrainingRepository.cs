using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Infrastructure.Repositories;

public class TrainingRepository: ITrainingRepository
{
    private readonly AppDbContext _context;

    public TrainingRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ScheduleSlot?> GetSlotByIdAsync(Guid slotId) =>
        await _context.ScheduleSlots.Include(s => s.Clients).FirstOrDefaultAsync(s => s.Id == slotId);

    public async Task<IEnumerable<ScheduleSlot>> GetClientTrainingsAsync(Guid clientId)
    {
        return await _context.ScheduleSlots
            .Include(s => s.Trainer)
            .Include(s => s.Clients)
            .Where(s => s.Clients.Any(c => c.Id == clientId))
            .ToListAsync();
    }

    public async Task RemoveClientFromSlotAsync(Guid clientId, Guid slotId)
    {
        var slot = await _context.ScheduleSlots
            .Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == slotId);

        if (slot == null)
            throw new KeyNotFoundException("Слот не знайдено.");

        var client = await _context.Clients.FindAsync(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клієнта не знайдено.");

        if (!slot.Clients.Remove(client))
            throw new InvalidOperationException("Клієнт не був записаний на цей слот.");

        await _context.SaveChangesAsync();
    }
    public async Task<Trainer?> GetTrainerWithClientsAsync(Guid trainerId) =>
        await _context.Trainers
            .Include(t => t.Clients)
            .FirstOrDefaultAsync(t => t.Id == trainerId);

    public async Task<IEnumerable<NutritionPlan>> GetNutritionPlansAsync(Guid trainerId) =>
        await _context.NutritionPlans
            .Where(p => p.TrainerId == trainerId)
            .ToListAsync();

    public async Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsAsync(Guid trainerId) =>
        await _context.WorkoutSessions
            .Where(w => w.TrainerId == trainerId)
            .ToListAsync();

    public async Task<NutritionPlan?> GetNutritionPlanByIdAsync(Guid planId) =>
        await _context.NutritionPlans.FirstOrDefaultAsync(p => p.Id == planId);

    public async Task AddNutritionPlanAsync(NutritionPlan plan)
    {
        _context.NutritionPlans.Add(plan);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNutritionPlanAsync(NutritionPlan plan)
    {
        _context.NutritionPlans.Update(plan);
        await _context.SaveChangesAsync();
    }
}