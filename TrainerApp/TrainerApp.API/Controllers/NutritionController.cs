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
    private readonly INutritionService _service;

    public NutritionController(INutritionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetTrainerPlansAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var plan = await _service.GetByIdAsync(id);
        return plan is null ? NotFound() : Ok(plan);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNutritionPlanDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return Ok(new { Message = "План створено", PlanId = id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, CreateNutritionPlanDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { Message = "План оновлено" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { Message = "План видалено" });
    }
}
