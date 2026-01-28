using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Interfaces;

public interface IPhotoService
{
    Task<Guid> UploadAsync(
        Guid userId,
        string role,
        Stream photoStream,
        string fileName,
        PhotoType type);

    Task<IEnumerable<Photo>> GetUserPhotosAsync(Guid userId);
}