using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Interfaces.Repositories;

public interface IWorkoutRepository
{
    Task<IEnumerable<WorkoutSession>> GetTrainerWorkoutsAsync(Guid trainerId);
    Task<WorkoutSession?> GetWorkoutByIdAsync(Guid trainerId, Guid workoutId);
    Task AddWorkoutAsync(WorkoutSession session);
    Task UpdateWorkoutAsync(WorkoutSession session);
    Task DeleteWorkoutAsync(WorkoutSession session);
}