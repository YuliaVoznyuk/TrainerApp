using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Interfaces;

public interface IPhotoService
{
    Task<Guid> UploadAsync(Guid userId, string role, string url, PhotoType type);
    Task<IEnumerable<Photo>> GetUserPhotosAsync(Guid userId);
}