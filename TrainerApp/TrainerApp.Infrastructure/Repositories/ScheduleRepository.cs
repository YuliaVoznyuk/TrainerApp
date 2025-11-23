using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly AppDbContext _context;

    public ScheduleRepository(AppDbContext context)
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
                s.IsOnline,
                Link = s.OnlineMeetingUrl
            })
            .ToListAsync();
    }

    public async Task<Guid> CreateSlotAsync(Guid trainerId, DateTime startAt, DateTime endAt, int maxClients, bool isOnline, string? link)
    {
        var slot = new ScheduleSlot
        {
            Id = Guid.NewGuid(),
            TrainerId = trainerId,
            StartAt = startAt,
            EndAt = endAt,
            MaxClients = maxClients,
            IsOnline = isOnline,
            OnlineMeetingUrl = link
        };

        if (isOnline && string.IsNullOrWhiteSpace(link))
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);
            slot.OnlineMeetingUrl = trainer?.Email != null
                ? $"facetime://{trainer.Email}"
                : $"facetime://{Guid.NewGuid()}";
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
            throw new InvalidOperationException("Already registered.");

        if (slot.Clients.Count >= slot.MaxClients)
            throw new InvalidOperationException("Slot is full.");

        var client = await _context.Clients.FirstAsync(c => c.Id == clientId);
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
            ?? throw new InvalidOperationException("Not registered.");

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