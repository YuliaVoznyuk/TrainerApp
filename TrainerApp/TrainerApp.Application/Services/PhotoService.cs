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
    private readonly IPhotoStorage _storage;
    public PhotoService(
        IPhotoRepository repo,
        IPhotoStorage storage)
    {
        _repo = repo;
        _storage = storage;
    }
    public async Task<Guid> UploadAsync(
        Guid userId,
        string role,
        Stream photoStream,
        string fileName,
        PhotoType type)
    {
        ValidatePhotoType(role, type);

        var url = await _storage.SaveAsync(
            photoStream,
            fileName,
            "photos");

        return await _repo.CreateAsync(userId, role, url, type);
    }



    public Task<IEnumerable<Photo>> GetUserPhotosAsync(Guid userId)
        => _repo.GetUserPhotosAsync(userId);

    private static void ValidatePhotoType(string role, PhotoType type)
    {
        if (role == "Client" &&
            type is not (PhotoType.Before or PhotoType.After or PhotoType.Personal))
            throw new ArgumentException($"Client cannot upload: {type}");

        if (role == "Trainer" &&
            type is not (PhotoType.Personal or PhotoType.Certificate))
            throw new ArgumentException($"Trainer cannot upload: {type}");
    }
}
