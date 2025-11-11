using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrainerApp.Application.Services;
using TrainerApp.Domain.Entities;

namespace TrainerApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Client")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly UserManager<User> _userManager;

    public ClientController(IClientService clientService, UserManager<User> userManager)
    {
        _clientService = clientService;
        _userManager = userManager;
    }

    private async Task<Guid> GetClientIdAsync()
    {
        var user = await _userManager.GetUserAsync(User) as Client;
        if (user == null)
            throw new UnauthorizedAccessException("Користувач не є клієнтом.");
        return user.Id;
    }

    [HttpGet("trainings")]
    public async Task<IActionResult> GetMyTrainings()
    {
        var clientId = await GetClientIdAsync();
        var result = await _clientService.GetMyTrainingsAsync(clientId);
        return Ok(result);
    }

    [HttpGet("nutrition")]
    public async Task<IActionResult> GetMyNutritionPlan()
    {
        var clientId = await GetClientIdAsync();
        var plan = await _clientService.GetMyNutritionPlanAsync(clientId);
        return Ok(plan);
    }

    [HttpDelete("cancel/{slotId:guid}")]
    public async Task<IActionResult> CancelTraining(Guid slotId)
    {
        var clientId = await GetClientIdAsync();
        await _clientService.CancelTrainingAsync(clientId, slotId);
        return Ok(new { Message = "Ви скасували запис." });
    }

    [HttpPost("progress-photo")]
    public async Task<IActionResult> UploadProgressPhoto([FromForm] IFormFile photo)
    {
        var clientId = await GetClientIdAsync();
        var id = await _clientService.UploadProgressPhotoAsync(clientId, photo);
        return Ok(new { Message = "Фото завантажено успішно.", PhotoId = id });
    }
}