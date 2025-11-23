using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Interfaces.Repositories;

public interface IPhotoRepository
{
    Task<Guid> AddPhotoAsync(Guid clientId, string url,PhotoType type);
    Task<IEnumerable<Photo>> GetClientPhotosAsync(Guid clientId);
    Task<IEnumerable<Photo>> GetTrainerPhotosAsync(Guid trainerId);
}