using TrainerApp.Application.Interfaces.FileStorage;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.UseCases.Client;

public class UploadProgressPhotoHandler
{
    private readonly IFileStorage _fileStorage;
    private readonly IPhotoRepository _photoRepository;

    public UploadProgressPhotoHandler(IFileStorage fileStorage, IPhotoRepository photoRepository)
    {
        _fileStorage = fileStorage;
        _photoRepository = photoRepository;
    }

    public async Task<Guid> HandleAsync(Guid userId, string role, Stream fileStream, string fileName, PhotoType type)
    {
        if (role != "Client")
            throw new UnauthorizedAccessException("Only clients can upload progress photos.");

        if (type is not (PhotoType.Before or PhotoType.After))
            throw new ArgumentException("Progress photos must be Before or After type.");

        var fileUrl = await _fileStorage.SaveFileAsync(fileStream, fileName, "progress");

        var photo = new Photo
        {
            Url = fileUrl,
            Type = type,
            UserId = userId
        };

        return await _photoRepository.AddAsync(photo);
    }
}