namespace TrainerApp.Application.DTOs;

public record ClientDto
{
    public Guid Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
}