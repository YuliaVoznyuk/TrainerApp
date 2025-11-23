using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Enums;

namespace TrainerApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoService;

    public PhotoController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    // 🔹 Отримати ID користувача з JWT Claims
    private Guid GetUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID in token.");
        }
        return userId;
    }

    // -----------------------------
    // CLIENT ACTIONS
    // -----------------------------

    [HttpPost("client/upload")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> UploadClientPhoto([FromForm] IFormFile photo, [FromQuery] PhotoType type = PhotoType.Personal)
    {
        if (photo == null || photo.Length == 0)
            return BadRequest("Photo is required.");

        // Тут можна додати логіку збереження файлу на диск або хмару
        var fileName = $"{Guid.NewGuid()}_{photo.FileName}";
        var filePath = Path.Combine("Uploads", fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        await using var stream = System.IO.File.Create(filePath);
        await photo.CopyToAsync(stream);

        var photoId = await _photoService.UploadClientPhotoAsync(GetUserId(), filePath, type);
        return Ok(new { Message = "Фото завантажено.", PhotoId = photoId });
    }

    [HttpGet("client")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetClientPhotos()
    {
        var photos = await _photoService.GetClientPhotosAsync(GetUserId());
        return Ok(photos);
    }

    // -----------------------------
    // TRAINER ACTIONS
    // -----------------------------

    [HttpPost("trainer/upload")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> UploadTrainerPhoto([FromForm] IFormFile photo, [FromQuery] PhotoType type = PhotoType.Personal)
    {
        if (photo == null || photo.Length == 0)
            return BadRequest("Photo is required.");

        var fileName = $"{Guid.NewGuid()}_{photo.FileName}";
        var filePath = Path.Combine("Uploads", fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        await using var stream = System.IO.File.Create(filePath);
        await photo.CopyToAsync(stream);

        var photoId = await _photoService.UploadTrainerPhotoAsync(GetUserId(), filePath, type);
        return Ok(new { Message = "Фото завантажено.", PhotoId = photoId });
    }

    [HttpGet("trainer")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> GetTrainerPhotos()
    {
        var photos = await _photoService.GetTrainerPhotosAsync(GetUserId());
        return Ok(photos);
    }
}