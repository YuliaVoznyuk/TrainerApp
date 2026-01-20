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
    public async Task<Guid> CreateAsync(Guid userId, string role, string url, PhotoType type)
    {
        var photo = new Photo
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Url = url,
            Type = type,
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
        return photo.Id;
    }
    public async Task<Guid> AddAsync(Photo photo)
    {
        await _context.Photos.AddAsync(photo);
        await _context.SaveChangesAsync();
        return photo.Id;
    }

    public async Task<IEnumerable<Photo>> GetUserPhotosAsync(Guid userId)
    {
        return await _context.Photos
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

}