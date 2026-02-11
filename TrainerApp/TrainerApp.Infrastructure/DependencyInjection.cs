using System.Reflection;
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
using TrainerApp.Infrastructure.Storage;

namespace TrainerApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAutoMapper(cfg =>
        {
            
        }, Assembly.GetExecutingAssembly());

        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<INutritionRepository, NutritionRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IPhotoStorage, S3PhotoStorage>();

       
        services.AddScoped<IClientRepository, ClientRepository>();

        
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}