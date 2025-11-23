using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Interfaces;

public interface IPhotoService
{
    Task<Guid> UploadClientPhotoAsync(Guid clientId, string url, PhotoType type);
    Task<Guid> UploadTrainerPhotoAsync(Guid trainerId, string url, PhotoType type);

    Task<IEnumerable<Photo>> GetClientPhotosAsync(Guid clientId);
    Task<IEnumerable<Photo>> GetTrainerPhotosAsync(Guid trainerId);
}