using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Services;

public class PhotoService : IPhotoService
{
    private readonly IPhotoRepository _repo;

    public PhotoService(IPhotoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> UploadAsync(Guid userId, string role, string url, PhotoType type)
    {
        ValidatePhotoType(role, type);

        var photo = new Photo
        {
            Url = url,
            Type = type,
            UserId = userId
        };

        return await _repo.AddAsync(photo);
    }

    public async Task<IEnumerable<Photo>> GetUserPhotosAsync(Guid userId)
    {
        return await _repo.GetUserPhotosAsync(userId);
    }

    private void ValidatePhotoType(string role, PhotoType type)
    {
        if (role == "Client")
        {
            if (type is not (PhotoType.Before or PhotoType.After or PhotoType.Personal))
                throw new ArgumentException($"Client cannot upload: {type}");
        }

        if (role == "Trainer")
        {
            if (type is not (PhotoType.Personal or PhotoType.Certificate))
                throw new ArgumentException($"Trainer cannot upload: {type}");
        }
    }
}
