using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Application.Services;

public class NutritionService : INutritionService
{
    private readonly AppDbContext _context;

    public NutritionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NutritionPlanDto>> GetTrainerPlansAsync(Guid trainerId)
    {
        var plans = await _context.NutritionPlans
            .Include(p => p.Items)
            .Where(p => p.TrainerId == trainerId)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();

        return plans.Select(MapToDto);
    }

    public async Task<NutritionPlanDto?> GetByIdAsync(Guid trainerId, Guid planId)
    {
        var plan = await _context.NutritionPlans
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.TrainerId == trainerId && p.Id == planId);

        return plan == null ? null : MapToDto(plan);
    }

    public async Task<Guid> CreateAsync(Guid trainerId, CreateNutritionPlanDto dto)
    {
        var plan = new NutritionPlan
        {
            Id = Guid.NewGuid(),
            TrainerId = trainerId,
            ClientId = dto.ClientId,
            Title = dto.Title,
            Notes = dto.Description,
            StartDate = DateTime.UtcNow,
            Items = dto.Items.Select(i => new NutritionItem
            {
                Id = Guid.NewGuid(),
                Name = i.Name,
                Calories = i.Calories,
                Protein = i.Protein,
                Carbs = i.Carbs,
                Fats = i.Fats,
                Description = i.Description,
            }).ToList()
        };

        _context.NutritionPlans.Add(plan);
        await _context.SaveChangesAsync();

        return plan.Id;
    }

    public async Task UpdateAsync(Guid trainerId, Guid planId, CreateNutritionPlanDto dto)
    {
        var plan = await _context.NutritionPlans
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == planId && p.TrainerId == trainerId);

        if (plan == null)
            throw new KeyNotFoundException("План харчування не знайдено.");

        plan.Title = dto.Title;
        plan.Notes = dto.Description;
        plan.ClientId = dto.ClientId;

        _context.NutritionItems.RemoveRange(plan.Items);

        plan.Items = dto.Items.Select(i => new NutritionItem
        {
            Id = Guid.NewGuid(),
            Name = i.Name,
            Protein = i.Protein,
            Carbs = i.Carbs,
            Fats = i.Fats,
            Description = i.Description,
            Calories = i.Calories
        }).ToList();

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid trainerId, Guid planId)
    {
        var plan = await _context.NutritionPlans
            .FirstOrDefaultAsync(p => p.Id == planId && p.TrainerId == trainerId);

        if (plan == null)
            throw new KeyNotFoundException("План харчування не знайдено.");

        _context.NutritionPlans.Remove(plan);
        await _context.SaveChangesAsync();
    }

    private static NutritionPlanDto MapToDto(NutritionPlan plan) => new()
    {
        Id = plan.Id,
        Title = plan.Title,
        Notes = plan.Notes,
        StartDate = plan.StartDate,
        Items = plan.Items.Select(i => new NutritionItemDto
        {
            Id = i.Id,
            Name = i.Name,
            Protein = i.Protein,
            Carbs = i.Carbs,
            Fats = i.Fats,
            Description = i.Description,
            Calories = i.Calories
        })
    };
}