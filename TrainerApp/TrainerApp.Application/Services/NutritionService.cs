using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Services;

public class NutritionService : INutritionService
{
    private readonly INutritionRepository _repository;

    public NutritionService(INutritionRepository repository)
    {
        _repository = repository;
    }

   public async Task<IEnumerable<NutritionPlanDto>> GetTrainerPlansAsync(Guid trainerId)
    {
        var plans = await _repository.GetTrainerPlansAsync(trainerId);
        return plans.Select(MapToDto);
    }

    public async Task<NutritionPlanDto?> GetByIdAsync(Guid trainerId, Guid planId)
    {
        var plan = await _repository.GetPlanByIdAsync(trainerId, planId);
        return plan == null ? null : MapToDto(plan);
    }

    public Task<Guid> CreateAsync(Guid trainerId, CreateNutritionPlanDto dto)
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

        return _repository.CreatePlanAsync(plan);
    }

    public async Task UpdateAsync(Guid trainerId, Guid planId, CreateNutritionPlanDto dto)
    {
        var plan = await _repository.GetByIdAsync(trainerId, planId)
                   ?? throw new KeyNotFoundException("План харчування не знайдено.");

        plan.Title = dto.Title;
        plan.Notes = dto.Description;
        plan.ClientId = dto.ClientId;

        plan.Items.Clear();
        foreach (var i in dto.Items)
        {
            plan.Items.Add(new NutritionItem
            {
                Id = Guid.NewGuid(),
                Name = i.Name,
                Calories = i.Calories,
                Protein = i.Protein,
                Carbs = i.Carbs,
                Fats = i.Fats,
                Description = i.Description
            });
        }

        await _repository.SaveChangesAsync();
    }

    public Task DeleteAsync(Guid trainerId, Guid planId) =>
        _repository.DeletePlanAsync(trainerId, planId);

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