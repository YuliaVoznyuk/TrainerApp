using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Infrastructure.Repositories;

public class NutritionRepository : INutritionRepository
{
    private readonly AppDbContext _context;

    public NutritionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NutritionPlan>> GetTrainerPlansAsync(Guid trainerId) =>
        await _context.NutritionPlans
            .Include(p => p.Items)
            .Where(p => p.TrainerId == trainerId)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();

    public async Task<NutritionPlan?> GetPlanByIdAsync(Guid trainerId, Guid planId) =>
        await _context.NutritionPlans
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.TrainerId == trainerId && p.Id == planId);

    public async Task<Guid> CreatePlanAsync(NutritionPlan plan)
    {
        _context.NutritionPlans.Add(plan);
        await _context.SaveChangesAsync();
        return plan.Id;
    }

    public async Task UpdatePlanAsync(NutritionPlan plan)
    {
        var existing = await _context.NutritionPlans
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == plan.Id && p.TrainerId == plan.TrainerId);

        if (existing == null)
            throw new KeyNotFoundException("План харчування не знайдено.");

        existing.Title = plan.Title;
        existing.Notes = plan.Notes;
        existing.ClientId = plan.ClientId;

        _context.NutritionItems.RemoveRange(existing.Items);

        existing.Items = plan.Items;

        await _context.SaveChangesAsync();
    }

    public async Task DeletePlanAsync(Guid trainerId, Guid planId)
    {
        var plan = await _context.NutritionPlans
            .FirstOrDefaultAsync(p => p.Id == planId && p.TrainerId == trainerId);

        if (plan == null)
            throw new KeyNotFoundException("План харчування не знайдено.");

        _context.NutritionPlans.Remove(plan);
        await _context.SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}