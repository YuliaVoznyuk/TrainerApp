using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Application.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ClientService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IEnumerable<ClientTrainingDto>> GetMyTrainingsAsync(Guid clientId)
    {
        var slots = await _context.ScheduleSlots
            .Include(s => s.Trainer)
            .Include(s => s.Clients)
            .Where(s => s.Clients.Any(c => c.Id == clientId))
            .OrderBy(s => s.StartAt)
            .ToListAsync();

        return slots.Select(s => new ClientTrainingDto
        {
            SlotId = s.Id,
            StartAt = s.StartAt,
            EndAt = s.EndAt,
            TrainerName = $"{s.Trainer.FirstName} {s.Trainer.LastName}"
        });
    }

    public async Task<ClientNutritionDto?> GetMyNutritionPlanAsync(Guid clientId)
    {
        var plan = await _context.NutritionPlans
            .FirstOrDefaultAsync(p => p.ClientId == clientId);

        return plan == null
            ? null
            : new ClientNutritionDto
            {
                Id = plan.Id,
                Title = plan.Title,
                Description = plan.Notes
            };
    }

    public async Task CancelTrainingAsync(Guid clientId, Guid slotId)
    {
        var slot = await _context.ScheduleSlots
            .Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == slotId);

        if (slot == null)
            throw new KeyNotFoundException("Слот не знайдено.");

        var client = await _context.Clients.FindAsync(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клієнта не знайдено.");

        if (!slot.Clients.Remove(client))
            throw new InvalidOperationException("Клієнт не був записаний на цей слот.");

        await _context.SaveChangesAsync();
    }

    public async Task<Guid> UploadProgressPhotoAsync(Guid clientId, IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
            throw new ArgumentException("Файл не завантажено.");

        var uploadsPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "progress");
        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await photo.CopyToAsync(stream);

        var progressPhoto = new Photo
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            Url = $"/progress/{fileName}",
            UploadedAt = DateTime.UtcNow
        };

        _context.Photos.Add(progressPhoto);
        await _context.SaveChangesAsync();

        return progressPhoto.Id;
    }
}