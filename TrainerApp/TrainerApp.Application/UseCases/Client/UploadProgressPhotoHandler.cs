using TrainerApp.Application.Interfaces.FileStorage;
using TrainerApp.Application.Interfaces.Repositories;
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

    public async Task<Guid> HandleAsync(Guid clientId, Stream fileStream, string fileName,PhotoType type)
    {
        var fileUrl = await _fileStorage.SaveFileAsync(fileStream, fileName, "progress");

        var photoId = await _photoRepository.AddPhotoAsync(clientId, fileUrl,type);
        return photoId;
    }
}