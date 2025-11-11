namespace TrainerApp.Domain.Entities;

public class ScheduleSlot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TrainerId { get; set; }  
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int MaxClients { get; set; } = 3;
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public bool IsOnline { get; set; }              
    public string? OnlineMeetingUrl { get; set; }
    public Trainer? Trainer { get; set; }
}