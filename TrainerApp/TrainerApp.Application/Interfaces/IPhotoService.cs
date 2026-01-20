using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrainerApp.Domain.Entities;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Application.Interfaces;

public interface IPhotoService
{
    Task<Guid> UploadAsync(Guid userId, string role, IFormFile photo, PhotoType type);

    Task<IEnumerable<Photo>> GetUserPhotosAsync(Guid userId);
}