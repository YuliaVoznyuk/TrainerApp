using Microsoft.AspNetCore.Hosting;
using TrainerApp.Application.Interfaces.FileStorage;

namespace TrainerApp.Infrastructure.Services;

public class FileStorageService : IFileStorage
{
    private readonly IWebHostEnvironment _env;

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder)
    {
        var uploadsPath = Path.Combine(_env.WebRootPath ?? "wwwroot", folder);
        Directory.CreateDirectory(uploadsPath);

        var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var filePath = Path.Combine(uploadsPath, newFileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(stream);

        return $"/{folder}/{newFileName}";
    }
}