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

    public Task UpdateAsync(Guid trainerId, Guid planId, CreateNutritionPlanDto dto)
    {
        var plan = new NutritionPlan
        {
            Id = planId,
            TrainerId = trainerId,
            ClientId = dto.ClientId,
            Title = dto.Title,
            Notes = dto.Description,
            Items = dto.Items.Select(i => new NutritionItem
            {
                Id = Guid.NewGuid(),
                Name = i.Name,
                Calories = i.Calories,
                Protein = i.Protein,
                Carbs = i.Carbs,
                Fats = i.Fats,
                Description = i.Description
            }).ToList()
        };

        return _repository.UpdatePlanAsync(plan);
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
    public Task<Guid> GetTrainerIdAsync(ClaimsPrincipal userClaims)
    {
        var trainerIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (trainerIdClaim == null || !Guid.TryParse(trainerIdClaim, out var trainerId))
            throw new UnauthorizedAccessException("Користувач не є тренером.");

        return Task.FromResult(trainerId);
    }
}