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
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;
        private readonly UserManager<User> _userManager;

        public TrainerController(ITrainerService trainerService, UserManager<User> userManager)
        {
            _trainerService = trainerService;
            _userManager = userManager;
        }

        private async Task<Guid> GetTrainerIdAsync()
        {
            var user = await _userManager.GetUserAsync(User) as Trainer;
            if (user == null)
                throw new UnauthorizedAccessException("Користувач не є тренером.");
            return user.Id;
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var trainerId = await GetTrainerIdAsync();
            var result = await _trainerService.GetClientsAsync(trainerId);
            return Ok(result);
        }

        [HttpGet("nutrition-plans")]
        public async Task<IActionResult> GetNutritionPlans()
        {
            var trainerId = await GetTrainerIdAsync();
            var result = await _trainerService.GetNutritionPlansAsync(trainerId);
            return Ok(result);
        }

        [HttpGet("workouts")]
        public async Task<IActionResult> GetWorkoutSessions()
        {
            var trainerId = await GetTrainerIdAsync();
            var result = await _trainerService.GetWorkoutSessionsAsync(trainerId);
            return Ok(result);
        }

        [HttpPost("nutrition-plans")]
        public async Task<IActionResult> AddOrUpdatePlan([FromBody] NutritionPlanDto dto)
        {
            var trainerId = await GetTrainerIdAsync();
            await _trainerService.AddOrUpdateNutritionPlanAsync(trainerId, dto);
            return Ok(new { Message = "План харчування успішно збережено." });
        }
    }
