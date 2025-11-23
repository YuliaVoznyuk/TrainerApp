using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;
using TrainerApp.Application.Interfaces.Repositories;

namespace TrainerApp.Application.Services;

public class TrainerService : ITrainerService
{
    private readonly ITrainingRepository _repository;
    private readonly IMapper _mapper;

    public TrainerService(ITrainingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<Guid> GetTrainerIdAsync(ClaimsPrincipal userClaims)
    {
        var idClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idClaim == null || !Guid.TryParse(idClaim, out var trainerId))
            throw new UnauthorizedAccessException("Користувач не є тренером.");

        return trainerId;
    }
    public async Task<IEnumerable<ClientDto>> GetClientsAsync(Guid trainerId)
    {
        var trainer = await _repository.GetTrainerWithClientsAsync(trainerId)
                      ?? throw new KeyNotFoundException("Тренера не знайдено.");

        return trainer.Clients.Select(c => _mapper.Map<ClientDto>(c)).ToList();
    }

    public async Task<IEnumerable<NutritionPlanDto>> GetNutritionPlansAsync(Guid trainerId)
    {
        var plans = await _repository.GetNutritionPlansAsync(trainerId);
        return plans.Select(p => _mapper.Map<NutritionPlanDto>(p)).ToList();
    }

    public async Task<IEnumerable<WorkoutSessionDto>> GetWorkoutSessionsAsync(Guid trainerId)
    {
        var sessions = await _repository.GetWorkoutSessionsAsync(trainerId);
        return sessions.Select(s => _mapper.Map<WorkoutSessionDto>(s)).ToList();
    }

    public async Task AddOrUpdateNutritionPlanAsync(Guid trainerId, NutritionPlanDto dto)
    {
        if (dto.Id == Guid.Empty)
        {
            var plan = _mapper.Map<NutritionPlan>(dto);
            plan.TrainerId = trainerId;
            await _repository.AddNutritionPlanAsync(plan);
        }
        else
        {
            var plan = await _repository.GetNutritionPlanByIdAsync(dto.Id)
                       ?? throw new KeyNotFoundException("План харчування не знайдено.");

            plan.Title = dto.Title;
            plan.Notes = dto.Notes;
            await _repository.UpdateNutritionPlanAsync(plan);
        }
    }
}