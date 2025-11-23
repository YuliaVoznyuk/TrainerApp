using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Infrastructure.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly AppDbContext _context;

    public WorkoutRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkoutSession>> GetTrainerWorkoutsAsync(Guid trainerId)
    {
        return await _context.WorkoutSessions
            .Include(w => w.ExerciseRecords)
            .Where(w => w.TrainerId == trainerId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<WorkoutSession?> GetWorkoutByIdAsync(Guid trainerId, Guid workoutId)
    {
        return await _context.WorkoutSessions
            .Include(w => w.ExerciseRecords)
            .FirstOrDefaultAsync(w => w.TrainerId == trainerId && w.Id == workoutId);
    }

    public async Task AddWorkoutAsync(WorkoutSession session)
    {
        await _context.WorkoutSessions.AddAsync(session);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWorkoutAsync(WorkoutSession session)
    {
        _context.WorkoutSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWorkoutAsync(WorkoutSession session)
    {
        _context.WorkoutSessions.Remove(session);
        await _context.SaveChangesAsync();
    }
}