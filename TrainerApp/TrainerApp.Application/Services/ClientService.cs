using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repo;
    private readonly IWebHostEnvironment _env;
    private readonly UserManager<User> _userManager;
    


    public ClientService(IClientRepository repo,UserManager<User> userManager, IWebHostEnvironment env)
    {
        _repo = repo;
        _userManager = userManager;
        _env = env;
    }
    public async Task<Guid> GetClientIdAsync(ClaimsPrincipal userClaims)
    {
        var client = await _userManager.GetUserAsync(userClaims) as Client
                     ?? throw new UnauthorizedAccessException("Користувач не є клієнтом.");
        return client.Id;
    }
    public async Task<IEnumerable<ClientTrainingDto>> GetMyTrainingsAsync(Guid clientId)
    {
        var slots = await _repo.GetClientTrainingsAsync(clientId);

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
        var plan = await _repo.GetClientNutritionPlanAsync(clientId);

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
        var slot = await _repo.GetSlotByIdAsync(slotId)
                   ?? throw new KeyNotFoundException("Слот не знайдено.");

        var client = await _repo.GetClientByIdAsync(clientId)
                     ?? throw new KeyNotFoundException("Клієнта не знайдено.");

        if (!slot.Clients.Remove(client))
            throw new InvalidOperationException("Клієнт не був записаний на цей слот.");

        await _repo.SaveChangesAsync();
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

        await _repo.AddProgressPhotoAsync(progressPhoto);
        await _repo.SaveChangesAsync();

        return progressPhoto.Id;
    }
}