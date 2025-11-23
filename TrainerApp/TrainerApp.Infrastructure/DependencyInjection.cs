using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.FileStorage;
using TrainerApp.Application.Interfaces.Repositories;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure.Jwt;
using TrainerApp.Infrastructure.Persistence;
using TrainerApp.Infrastructure.Repositories;
using TrainerApp.Infrastructure.Services;

namespace TrainerApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        // Database (EF Core)
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        // ASP.NET Identity
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        // 🔹 Реєструємо реалізації інфраструктурних сервісів
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<INutritionRepository, NutritionRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IFileStorage, FileStorageService>();

        // JWT generator
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
