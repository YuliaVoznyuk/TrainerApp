using Microsoft.EntityFrameworkCore;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Infrastructure.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly AppDbContext _context;

    public PhotoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddPhotoAsync(Guid ownerId, string url, PhotoType type)
    {
        var photo = new Photo
        {
            Url = url,
            Type = type,
            UploadedAt = DateTime.UtcNow
        };

       
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        return photo.Id;
    }

    public async Task<IEnumerable<Photo>> GetClientPhotosAsync(Guid clientId)
    {
        return await _context.Photos
            .Where(p => p.ClientId == clientId)
            .OrderByDescending(p => p.UploadedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Photo>> GetTrainerPhotosAsync(Guid trainerId)
    {
        return await _context.Photos
            .Where(p => p.TrainerId == trainerId)
            .OrderByDescending(p => p.UploadedAt)
            .ToListAsync();
    }
}