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
    private readonly IFileStorage _fileStorage;

    public PhotoController(IPhotoService photoService, IFileStorage fileStorage)
    {
        _photoService = photoService;
        _fileStorage = fileStorage;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.Claims.First(c => c.Type == "id").Value);

    private string GetRole() =>
        User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile photo, [FromQuery] PhotoType type)
    {
        if (photo == null) return BadRequest("Photo is required");

        // контролер сам парсить Claims
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var role = User.FindFirstValue(ClaimTypes.Role);

        var id = await _photoService.UploadAsync(userId, role, photo, type);

        return Ok(new { PhotoId = id });
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var photos = await _photoService.GetUserPhotosAsync(GetUserId());
        return Ok(photos);
    }
}