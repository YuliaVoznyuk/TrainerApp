using System.Security.Claims;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Services;

public class NutritionService : INutritionService
{
   private readonly INutritionRepository _repository;
    private readonly ITrainerContext _trainerContext;

    public NutritionService(
        INutritionRepository repository,
        ITrainerContext trainerContext)
    {
        _repository = repository;
        _trainerContext = trainerContext;
    }

    public async Task<IEnumerable<NutritionPlanDto>> GetTrainerPlansAsync()
    {
        var plans = await _repository
            .GetTrainerPlansAsync(_trainerContext.TrainerId);

        return plans.Adapt<IEnumerable<NutritionPlanDto>>();
    }

    public async Task<NutritionPlanDto?> GetByIdAsync(Guid planId)
    {
        var plan = await _repository
            .GetPlanByIdAsync(_trainerContext.TrainerId, planId);

        return plan?.Adapt<NutritionPlanDto>();
    }

    public async Task<Guid> CreateAsync(CreateNutritionPlanDto dto)
    {
        var plan = dto.Adapt<NutritionPlan>();

        plan.Id = Guid.NewGuid();
        plan.TrainerId = _trainerContext.TrainerId;
        plan.StartDate = DateTime.UtcNow;

        plan.Items = dto.Items
            .Select(i =>
            {
                var item = i.Adapt<NutritionItem>();
                item.Id = Guid.NewGuid();
                return item;
            })
            .ToList();

        await _repository.CreatePlanAsync(plan);
        return plan.Id;
    }

    public async Task UpdateAsync(Guid planId, CreateNutritionPlanDto dto)
    {
        var plan = await _repository
                       .GetByIdAsync(_trainerContext.TrainerId, planId)
                   ?? throw new KeyNotFoundException("План харчування не знайдено");

        dto.Adapt(plan);

        plan.Items.Clear();

        foreach (var i in dto.Items)
        {
            var item = i.Adapt<NutritionItem>();
            item.Id = Guid.NewGuid();
            plan.Items.Add(item);
        }

        await _repository.SaveChangesAsync();
    }

    public Task DeleteAsync(Guid planId)
    {
        return _repository.DeletePlanAsync(
            _trainerContext.TrainerId,
            planId);
    }
}