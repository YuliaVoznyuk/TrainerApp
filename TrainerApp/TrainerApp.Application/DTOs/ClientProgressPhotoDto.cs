namespace TrainerApp.Application.DTOs;

public class ClientProgressPhotoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}