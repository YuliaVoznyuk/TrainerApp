using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Interfaces.Repositories;

public interface IPhotoRepository
{
    Task<Guid> AddAsync(Photo photo);
    Task<Guid> CreateAsync(Guid userId, string role, string url, PhotoType type);

    Task<IEnumerable<Photo>> GetUserPhotosAsync(Guid userId);
}