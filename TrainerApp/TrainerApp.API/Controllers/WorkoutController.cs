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

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var trainerId = await _workoutService.GetTrainerIdAsync(User);
        var workouts = await _workoutService.GetTrainerWorkoutsAsync(trainerId);
        return Ok(workouts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var trainerId = await _workoutService.GetTrainerIdAsync(User);
        var workout = await _workoutService.GetWorkoutByIdAsync(trainerId, id);
        return workout == null ? NotFound() : Ok(workout);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkoutDto dto)
    {
        var trainerId = await _workoutService.GetTrainerIdAsync(User);
        var id = await _workoutService.CreateWorkoutAsync(trainerId, dto);
        return Ok(new { Message = "Тренування створено.", WorkoutId = id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateWorkoutDto dto)
    {
        var trainerId = await _workoutService.GetTrainerIdAsync(User);
        await _workoutService.UpdateWorkoutAsync(trainerId, id, dto);
        return Ok(new { Message = "Тренування оновлено." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var trainerId = await _workoutService.GetTrainerIdAsync(User);
        await _workoutService.DeleteWorkoutAsync(trainerId, id);
        return Ok(new { Message = "Тренування видалено." });
    }
}