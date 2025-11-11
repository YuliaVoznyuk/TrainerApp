namespace TrainerApp.Application.DTOs;

public class ClientTrainingDto
{
    public Guid SlotId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string TrainerName { get; set; } = string.Empty;
    public bool IsUpcoming => StartAt > DateTime.UtcNow;
}