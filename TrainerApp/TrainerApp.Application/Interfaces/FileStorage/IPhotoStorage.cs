namespace TrainerApp.Application.Interfaces.FileStorage;

public interface IPhotoStorage
{
    Task<string> SaveAsync(
        Stream content,
        string fileName,
        string folder);
}