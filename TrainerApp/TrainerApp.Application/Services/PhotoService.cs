using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Services;

public class PhotoService : IPhotoService
{
    private readonly IPhotoRepository _photoRepository;

    public PhotoService(IPhotoRepository photoRepository)
    {
        _photoRepository = photoRepository;
    }

    public async Task<Guid> UploadClientPhotoAsync(Guid clientId, string url, PhotoType type)
    {
        return await _photoRepository.AddPhotoAsync(clientId, url, type);
    }

    public async Task<Guid> UploadTrainerPhotoAsync(Guid trainerId, string url, PhotoType type)
    {
        return await _photoRepository.AddPhotoAsync(trainerId, url, type);
    }

    public Task<IEnumerable<Photo>> GetClientPhotosAsync(Guid clientId) =>
        _photoRepository.GetClientPhotosAsync(clientId);

    public Task<IEnumerable<Photo>> GetTrainerPhotosAsync(Guid trainerId) =>
        _photoRepository.GetTrainerPhotosAsync(trainerId);
}