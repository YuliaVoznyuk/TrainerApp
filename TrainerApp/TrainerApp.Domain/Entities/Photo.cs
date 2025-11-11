using TrainerApp.Domain.Enums;

namespace TrainerApp.Domain.Entities;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = null!;
    public PhotoType Type { get; set; }

    public Guid? ClientId { get; set; }
    public Guid? TrainerId { get; set; }

    public Client? Client { get; set; }
    public Trainer? Trainer { get; set; }
    public DateTime UploadedAt { get; set; }
}