using Microsoft.AspNetCore.Http;
using TrainerApp.Application.DTOs;

namespace TrainerApp.Application.Services;

public interface IClientService
{
    Task<IEnumerable<ClientTrainingDto>> GetMyTrainingsAsync(Guid clientId);
    Task<ClientNutritionDto?> GetMyNutritionPlanAsync(Guid clientId);
    Task CancelTrainingAsync(Guid clientId, Guid slotId);
    Task<Guid> UploadProgressPhotoAsync(Guid clientId, IFormFile photo);
}