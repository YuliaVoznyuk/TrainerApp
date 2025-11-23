namespace TrainerApp.Application.Interfaces.FileStorage;

public interface IFileStorage
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder);

}