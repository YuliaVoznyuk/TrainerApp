using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Application.Services;

public class TrainerService : ITrainerService
{
    private readonly AppDbContext _context;

    public TrainerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientDto>> GetClientsAsync(Guid trainerId)
    {
        var trainer = await _context.Trainers
            .Include(t => t.Clients)
            .FirstOrDefaultAsync(t => t.Id == trainerId)
            ?? throw new KeyNotFoundException("Тренера не знайдено.");

        return trainer.Clients.Select(c => new ClientDto
        {
            Id = c.Id,
            FullName = $"{c.FirstName} {c.LastName}",
            Email = c.Email ?? string.Empty,
            Birthdate = c.Birthdate
        }).ToList();
    }

    public async Task<IEnumerable<NutritionPlanDto>> GetNutritionPlansAsync(Guid trainerId)
    {
        var plans = await _context.NutritionPlans
            .Where(p => p.TrainerId == trainerId)
            .ToListAsync();

        return plans.Select(p => new NutritionPlanDto
        {
            Id = p.Id,
            Title = p.Title,
            Notes = p.Notes,
            ClientId = p.ClientId
        });
    }

    public async Task<IEnumerable<WorkoutSessionDto>> GetWorkoutSessionsAsync(Guid trainerId)
    {
        var sessions = await _context.WorkoutSessions
            .Where(s => s.TrainerId == trainerId)
            .ToListAsync();

        return sessions.Select(s => new WorkoutSessionDto
        {
            Id = s.Id,
            Date = s.Date,
            ClientId = s.ClientId,
            Notes = s.Notes ?? ""
        });
    }

    public async Task AddOrUpdateNutritionPlanAsync(Guid trainerId, NutritionPlanDto dto)
    {
        var clientExists = await _context.Clients.AnyAsync(c => c.Id == dto.ClientId);
        if (!clientExists)
            throw new KeyNotFoundException("Клієнта не знайдено.");

        NutritionPlan? plan = null;

        if (dto.Id != Guid.Empty)
        {
            plan = await _context.NutritionPlans.FirstOrDefaultAsync(p => p.Id == dto.Id);
            if (plan == null)
                throw new KeyNotFoundException("План не знайдено.");
            
            plan.Title = dto.Title;
            plan.Notes = dto.Notes;
        }
        else
        {
            plan = new NutritionPlan
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Notes = dto.Notes,
                ClientId = dto.ClientId,
                TrainerId = trainerId
            };
            _context.NutritionPlans.Add(plan);
        }

        await _context.SaveChangesAsync();
    }
}