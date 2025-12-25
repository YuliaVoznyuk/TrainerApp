using System.Reflection;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.Extensions.DependencyInjection;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Services;
using TrainerApp.Application.UseCases.Auth;
using TrainerApp.Application.UseCases.Client;

namespace TrainerApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            
        }, Assembly.GetExecutingAssembly());
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<INutritionService, NutritionService>();

        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();

        services.AddScoped<GetClientTrainingsHandler>();
        
        services.AddScoped<CancelTrainingHandler>();
        services.AddScoped<UploadProgressPhotoHandler>();

        services.AddScoped<IAuthService, AuthService>();

        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}