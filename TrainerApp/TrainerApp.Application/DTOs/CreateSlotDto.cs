namespace TrainerApp.Application.DTOs;

public record CreateSlotDto(DateTime StartAt, DateTime EndAt, int MaxClients, string? link,bool IsOnline =false);
