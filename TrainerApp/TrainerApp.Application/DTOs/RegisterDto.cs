namespace TrainerApp.Application.DTOs;

public record RegisterDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateTime Birthdate,
    string Role);