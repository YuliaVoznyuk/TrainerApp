using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.FileStorage;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Services;

public class PhotoService : IPhotoService
{
    private readonly IPhotoRepository _repo;
    private readonly IFileStorage _fileStorage;
    public PhotoService(IPhotoRepository repo, IFileStorage fileStorage)
    {
        _repo = repo;
        _fileStorage = fileStorage;

    }

    public async Task<Guid> UploadAsync(
        Guid userId,
        string role,
        IFormFile photo,
        PhotoType type)
    {
        ValidatePhotoType(role, type);

        var url = await _fileStorage.SaveFileAsync(
            photo.OpenReadStream(),
            photo.FileName,
            "uploads");

        return await _repo.CreateAsync(userId, role, url, type);
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
