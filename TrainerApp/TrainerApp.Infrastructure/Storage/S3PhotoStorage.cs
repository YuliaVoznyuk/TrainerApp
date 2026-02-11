using TrainerApp.Application.Interfaces.FileStorage;

namespace TrainerApp.Infrastructure.Storage;

public class S3PhotoStorage : IPhotoStorage
{
    public async Task<string> SaveAsync(
        Stream content,
        string fileName,
        string folder)
    {
        // AWS S3 upload logic
        // return public URL
        return $"https://s3.amazonaws.com/bucket/{folder}/{fileName}";
    }
}