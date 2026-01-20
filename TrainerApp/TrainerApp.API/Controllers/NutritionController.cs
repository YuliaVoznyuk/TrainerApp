using System.Security.Claims;
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
public class NutritionController : ControllerBase
{
    private readonly INutritionService _nutritionService;

    public NutritionController(INutritionService nutritionService)
    {
        _nutritionService = nutritionService;
        
    }
    private Guid GetTrainerId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }


   

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var trainerId = GetTrainerId();
        var plans = await _nutritionService.GetTrainerPlansAsync(trainerId);
        return Ok(plans);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var trainerId = GetTrainerId();
        var plan = await _nutritionService.GetByIdAsync(trainerId, id);
        return plan == null ? NotFound() : Ok(plan);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNutritionPlanDto dto)
    {
        var trainerId = GetTrainerId();
        var id = await _nutritionService.CreateAsync(trainerId, dto);
        return Ok(new { Message = "План створено", PlanId = id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateNutritionPlanDto dto)
    {
        var trainerId = GetTrainerId();
        await _nutritionService.UpdateAsync(trainerId, id, dto);
        return Ok(new { Message = "План оновлено" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var trainerId = GetTrainerId();
        await _nutritionService.DeleteAsync(trainerId, id);
        return Ok(new { Message = "План видалено" });
    }
}