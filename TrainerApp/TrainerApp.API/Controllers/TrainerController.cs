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

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var trainerId = await _trainerService.GetTrainerIdAsync(User);
            var result = await _trainerService.GetClientsAsync(trainerId);
            return Ok(result);
        }


        [HttpGet("nutrition-plans")]
        public async Task<IActionResult> GetNutritionPlans()
        {
            var trainerId = await _trainerService.GetTrainerIdAsync(User);
            var result = await _trainerService.GetNutritionPlansAsync(trainerId);
            return Ok(result);
        }
        [HttpGet("workouts")]
        public async Task<IActionResult> GetWorkoutSessions()
        {
            var trainerId = await _trainerService.GetTrainerIdAsync(User);
            var result = await _trainerService.GetWorkoutSessionsAsync(trainerId);
            return Ok(result);
        }

        [HttpPost("nutrition-plans")]
        public async Task<IActionResult> AddOrUpdatePlan([FromBody] NutritionPlanDto dto)
        {
            var trainerId = await _trainerService.GetTrainerIdAsync(User);
            await _trainerService.AddOrUpdateNutritionPlanAsync(trainerId, dto);
            return Ok(new { Message = "План харчування успішно збережено." });
        }
    }
