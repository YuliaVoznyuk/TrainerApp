using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly AppDbContext _context;

    public ScheduleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetAllSlotsAsync()
    {
        return await _context.ScheduleSlots
            .Include(s => s.Clients)
            .OrderBy(s => s.StartAt)
            .Select(s => new
            {
                s.Id,
                s.StartAt,
                s.EndAt,
                s.MaxClients,
                CurrentClients = s.Clients.Count,
                IsOnline = s.IsOnline,
                Link = s.OnlineMeetingUrl

            })
            .ToListAsync();
    }

    public async Task<Guid> CreateSlotAsync(Guid trainerId, DateTime startAt, DateTime endAt, int maxClient,bool isOnline = false, string? link = null)
    {
        var slot = new ScheduleSlot
        {
            Id = Guid.NewGuid(),
            TrainerId = trainerId,
            StartAt = startAt,
            EndAt = endAt,
            MaxClients = maxClient,
            IsOnline = isOnline,
            OnlineMeetingUrl = link

        };
        if (isOnline && string.IsNullOrWhiteSpace(slot.OnlineMeetingUrl))
        {
            var trainer = await _context.Users.OfType<Trainer>().FirstOrDefaultAsync(t => t.Id == trainerId);
            if (trainer != null && !string.IsNullOrWhiteSpace(trainer.Email))
            {
                slot.OnlineMeetingUrl = $"facetime://{trainer.Email}";
            }
            else
            {
                slot.OnlineMeetingUrl = $"facetime://{Guid.NewGuid()}";
            }
        }


        _context.ScheduleSlots.Add(slot);
        await _context.SaveChangesAsync();
        return slot.Id;
    }

    public async Task JoinSlotAsync(Guid clientId, Guid slotId)
    {
        var slot = await _context.ScheduleSlots
            .Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == slotId)
            ?? throw new KeyNotFoundException("Slot not found.");

        if (slot.Clients.Any(c => c.Id == clientId))
            throw new InvalidOperationException("You are already registered.");

        if (slot.Clients.Count >= slot.MaxClients)
            throw new InvalidOperationException("Slot is full.");

        var client = await _context.Users.OfType<Client>().FirstAsync(c => c.Id == clientId);
        slot.Clients.Add(client);
        await _context.SaveChangesAsync();
    }

    public async Task LeaveSlotAsync(Guid clientId, Guid slotId)
    {
        var slot = await _context.ScheduleSlots
            .Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == slotId)
            ?? throw new KeyNotFoundException("Slot not found.");

        var client = slot.Clients.FirstOrDefault(c => c.Id == clientId)
                     ?? throw new InvalidOperationException("You are not registered.");

        slot.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSlotAsync(Guid trainerId, Guid slotId)
    {
        var slot = await _context.ScheduleSlots
            .FirstOrDefaultAsync(s => s.Id == slotId && s.TrainerId == trainerId)
            ?? throw new KeyNotFoundException("Slot not found or not yours.");

        _context.ScheduleSlots.Remove(slot);
        await _context.SaveChangesAsync();
    }
}