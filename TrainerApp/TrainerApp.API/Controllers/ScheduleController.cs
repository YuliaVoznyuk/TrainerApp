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

    public ScheduleController(IScheduleService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllSlotsAsync());

    [HttpPost]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> Create([FromBody] CreateSlotDto dto)
    {
        var trainerId = await _service.GetTrainerIdAsync(User);
        var id = await _service.CreateSlotAsync(trainerId, dto.StartAt, dto.EndAt, dto.MaxClients, dto.IsOnline, dto.link);
        return Ok(new { Message = "Slot created", Id = id });
    }

    [HttpPost("{slotId:guid}/join")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Join(Guid slotId)
    {
        var clientId = await _service.GetClientIdAsync(User);
        await _service.JoinSlotAsync(clientId, slotId);
        return Ok(new { Message = "Joined slot successfully." });
    }

    [HttpDelete("{slotId:guid}/leave")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Leave(Guid slotId)
    {
        var clientId = await _service.GetClientIdAsync(User);
        await _service.LeaveSlotAsync(clientId, slotId);
        return Ok(new { Message = "Left slot successfully." });
    }


    [HttpDelete("{slotId:guid}")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> Delete(Guid slotId)
    {
        var trainerId = await _service.GetTrainerIdAsync(User);
        await _service.DeleteSlotAsync(trainerId, slotId);
        return Ok(new { Message = "Slot deleted." });
    }
}