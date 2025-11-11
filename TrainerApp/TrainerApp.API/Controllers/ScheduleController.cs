using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _service;
    private readonly UserManager<User> _userManager;

    public ScheduleController(IScheduleService service, UserManager<User> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllSlotsAsync());

    [HttpPost]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> Create([FromBody] CreateSlotDto dto)
    {
        var trainer = await _userManager.GetUserAsync(User) as Trainer
                      ?? throw new UnauthorizedAccessException("User is not a trainer.");

        var id = await _service.CreateSlotAsync(trainer.Id, dto.StartAt, dto.EndAt, dto.MaxClients, dto.IsOnline, dto.link);
        return Ok(new { Message = "Slot created", Id = id });
    }

    [HttpPost("{slotId:guid}/join")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Join(Guid slotId)
    {
        var client = await _userManager.GetUserAsync(User) as Client
                     ?? throw new UnauthorizedAccessException("User is not a client.");

        await _service.JoinSlotAsync(client.Id, slotId);
        return Ok(new { Message = "Joined slot successfully." });
    }

    [HttpDelete("{slotId:guid}/leave")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Leave(Guid slotId)
    {
        var client = await _userManager.GetUserAsync(User) as Client
                     ?? throw new UnauthorizedAccessException("User is not a client.");

        await _service.LeaveSlotAsync(client.Id, slotId);
        return Ok(new { Message = "Left slot successfully." });
    }

    [HttpDelete("{slotId:guid}")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> Delete(Guid slotId)
    {
        var trainer = await _userManager.GetUserAsync(User) as Trainer
                      ?? throw new UnauthorizedAccessException("User is not a trainer.");

        await _service.DeleteSlotAsync(trainer.Id, slotId);
        return Ok(new { Message = "Slot deleted." });
    }
}