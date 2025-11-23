using System.Security.Claims;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Interfaces;

public interface IWorkoutService
{
    Task<Guid> GetTrainerIdAsync(ClaimsPrincipal userClaims);

    Task<IEnumerable<WorkoutSessionDto>> GetTrainerWorkoutsAsync(Guid trainerId);
    Task<WorkoutSessionDto?> GetWorkoutByIdAsync(Guid trainerId, Guid workoutId);
    Task<Guid> CreateWorkoutAsync(Guid trainerId, CreateWorkoutDto dto);
    Task UpdateWorkoutAsync(Guid trainerId, Guid workoutId, CreateWorkoutDto dto);
    Task DeleteWorkoutAsync(Guid trainerId, Guid workoutId);
}