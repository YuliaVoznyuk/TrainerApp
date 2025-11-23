using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _repository;

    private readonly IMapper _mapper;

    public WorkoutService(IWorkoutRepository repository, IMapper mapper)
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

    public async Task<IEnumerable<WorkoutSessionDto>> GetTrainerWorkoutsAsync(Guid trainerId)
    {
        var sessions = await _repository.GetTrainerWorkoutsAsync(trainerId);
        return sessions.Select(s => _mapper.Map<WorkoutSessionDto>(s));
    }

    public async Task<WorkoutSessionDto?> GetWorkoutByIdAsync(Guid trainerId, Guid workoutId)
    {
        var workout = await _repository.GetWorkoutByIdAsync(trainerId, workoutId);
        return workout == null ? null : _mapper.Map<WorkoutSessionDto>(workout);
    }

    public async Task<Guid> CreateWorkoutAsync(Guid trainerId, CreateWorkoutDto dto)
    {
        var workout = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            TrainerId = trainerId,
            ClientId = dto.ClientId,
            Title = dto.Title,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            ExerciseRecords = dto.Exercises.Select(e => new ExerciseRecord
            {
                Id = Guid.NewGuid(),
                Name = e.Name,
                Sets = e.Sets,
                Reps = e.Reps,
                WeightKg = e.Weight
            }).ToList()
        };

        await _repository.AddWorkoutAsync(workout);

        return workout.Id;
    }

    public async Task UpdateWorkoutAsync(Guid trainerId, Guid workoutId, CreateWorkoutDto dto)
    {
        var workout = await _repository.GetWorkoutByIdAsync(trainerId, workoutId)
                      ?? throw new KeyNotFoundException("Тренування не знайдено.");

        workout.Title = dto.Title;
        workout.Notes = dto.Notes;
        workout.ClientId = dto.ClientId;

        workout.ExerciseRecords = dto.Exercises.Select(e => new ExerciseRecord
        {
            Id = Guid.NewGuid(),
            Name = e.Name,
            Sets = e.Sets,
            Reps = e.Reps,
            WeightKg = e.Weight
        }).ToList();

        await _repository.UpdateWorkoutAsync(workout);
    }
    public async Task DeleteWorkoutAsync(Guid trainerId, Guid workoutId)
    {
        var workout = await _repository.GetWorkoutByIdAsync(trainerId, workoutId)
                      ?? throw new KeyNotFoundException("Тренування не знайдено.");

        await _repository.DeleteWorkoutAsync(workout);
    }
}