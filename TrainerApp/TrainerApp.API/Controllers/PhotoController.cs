using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.FileStorage;
using TrainerApp.Domain.Enums;

namespace TrainerApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoService;
    private readonly IPhotoStorage _photoStorage;

    public PhotoController(IPhotoService photoService, IPhotoStorage photoStorage)
    {
        _photoService = photoService;
        _photoStorage = photoStorage;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.Claims.First(c => c.Type == "id").Value);

    private string GetRole() =>
        User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

    [HttpPost("upload")]
    [HttpPost]
    public async Task<IActionResult> UploadPhoto(
        IFormFile photo,
        PhotoType type)
    {
        var userId = GetUserId();
        var role = User.FindFirstValue(ClaimTypes.Role)!;

        var id = await _photoService.UploadAsync(
            userId,
            role,
            photo.OpenReadStream(),
            photo.FileName,
            type);

        return Ok(id);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var photos = await _photoService.GetUserPhotosAsync(GetUserId());
        return Ok(photos);
    }
}