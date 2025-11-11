namespace TrainerApp.Application.DTOs;

public record ScheduleSlotDto(Guid Id,DateTime StartAt,DateTime EndAt ,int MaxClients ,int ClientCount ,bool IsOnline,string? OnlineMeetingUrl );