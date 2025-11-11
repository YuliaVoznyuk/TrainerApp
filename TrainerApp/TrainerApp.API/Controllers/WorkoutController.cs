using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;

namespace TrainerApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Trainer")]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _workoutService;
    private readonly UserManager<User> _userManager;

    public WorkoutController(IWorkoutService workoutService, UserManager<User> userManager)
    {
        _workoutService = workoutService;
        _userManager = userManager;
    }

    private async Task<Guid> GetTrainerIdAsync()
    {
        var user = await _userManager.GetUserAsync(User) as Trainer;
        if (user == null)
            throw new UnauthorizedAccessException("Користувач не є тренером.");
        return user.Id;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var trainerId = await GetTrainerIdAsync();
        var workouts = await _workoutService.GetTrainerWorkoutsAsync(trainerId);
        return Ok(workouts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var trainerId = await GetTrainerIdAsync();
        var workout = await _workoutService.GetWorkoutByIdAsync(trainerId, id);
        return workout == null ? NotFound() : Ok(workout);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkoutDto dto)
    {
        var trainerId = await GetTrainerIdAsync();
        var id = await _workoutService.CreateWorkoutAsync(trainerId, dto);
        return Ok(new { Message = "Тренування створено.", WorkoutId = id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateWorkoutDto dto)
    {
        var trainerId = await GetTrainerIdAsync();
        await _workoutService.UpdateWorkoutAsync(trainerId, id, dto);
        return Ok(new { Message = "Тренування оновлено." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var trainerId = await GetTrainerIdAsync();
        await _workoutService.DeleteWorkoutAsync(trainerId, id);
        return Ok(new { Message = "Тренування видалено." });
    }
}