using Microsoft.AspNetCore.Identity;
using TrainerApp.Domain.Enums;

namespace TrainerApp.Domain.Entities;

public abstract class User: IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Middlename { get; set; }
    public DateTime Birthdate { get; set; }
}