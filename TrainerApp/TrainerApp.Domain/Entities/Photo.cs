using TrainerApp.Domain.Enums;

namespace TrainerApp.Domain.Entities;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = null!;
    public PhotoType Type { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
}