using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Persistence;

namespace TrainerApp.Infrastructure.Seed;

public class AppUserSeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public AppUserSeeder(
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<User> userManager,
        AppDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    public async Task SeedAsync()
    {
        var roles = new[] { "Trainer", "Client" };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        if (!await _userManager.Users.OfType<Trainer>().AnyAsync())
        {
            var trainer = new Trainer
            {
                UserName = "trainer@example.com",
                Email = "trainer@example.com",
                FirstName = "John",
                LastName = "Trainer",
                Middlename = "o",
                Bio = "Сертифікований персональний тренер",
                AvatarUrl = null,
                Birthdate = DateTime.SpecifyKind(new DateTime(1995, 8, 12), DateTimeKind.Utc),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(trainer, "Trainer123!");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(trainer, "Trainer");
            }
        }
    }
}