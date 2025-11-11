using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Application.Services;

public class WorkoutService : IWorkoutService
{
    private readonly AppDbContext _context;

    public WorkoutService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkoutSessionDto>> GetTrainerWorkoutsAsync(Guid trainerId)
    {
        var sessions = await _context.WorkoutSessions
            .Include(w => w.ExerciseRecords)
            .Where(w => w.TrainerId == trainerId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();

        return sessions.Select(MapToDto);
    }

    public async Task<WorkoutSessionDto?> GetWorkoutByIdAsync(Guid trainerId, Guid workoutId)
    {
        var session = await _context.WorkoutSessions
            .Include(w => w.ExerciseRecords)
            .FirstOrDefaultAsync(w => w.TrainerId == trainerId && w.Id == workoutId);

        return session == null ? null : MapToDto(session);
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

        _context.WorkoutSessions.Add(workout);
        await _context.SaveChangesAsync();

        return workout.Id;
    }

    public async Task UpdateWorkoutAsync(Guid trainerId, Guid workoutId, CreateWorkoutDto dto)
    {
        var workout = await _context.WorkoutSessions
            .Include(w => w.ExerciseRecords)
            .FirstOrDefaultAsync(w => w.Id == workoutId && w.TrainerId == trainerId);

        if (workout == null)
            throw new KeyNotFoundException("Тренування не знайдено.");

        workout.Title = dto.Title;
        workout.Notes = dto.Notes;
        workout.ClientId = dto.ClientId;

        _context.ExerciseRecords.RemoveRange(workout.ExerciseRecords);

        workout.ExerciseRecords = dto.Exercises.Select(e => new ExerciseRecord
        {
            Id = Guid.NewGuid(),
            Name = e.Name,
            Sets = e.Sets,
            Reps = e.Reps,
            WeightKg = e.Weight
        }).ToList();

        await _context.SaveChangesAsync();
    }

    public async Task DeleteWorkoutAsync(Guid trainerId, Guid workoutId)
    {
        var workout = await _context.WorkoutSessions
            .FirstOrDefaultAsync(w => w.Id == workoutId && w.TrainerId == trainerId);

        if (workout == null)
            throw new KeyNotFoundException("Тренування не знайдено.");

        _context.WorkoutSessions.Remove(workout);
        await _context.SaveChangesAsync();
    }

    private static WorkoutSessionDto MapToDto(WorkoutSession w) => new()
    {
        Id = w.Id,
        Title = w.Title,
        Notes = w.Notes,
        CreatedAt = w.CreatedAt,
        Exercises = w.ExerciseRecords.Select(e => new ExerciseRecordDto
        {
            Id = e.Id,
            Name = e.Name,
            Sets = e.Sets,
            Reps = e.Reps,
            Weight = e.WeightKg
        })
    };
}