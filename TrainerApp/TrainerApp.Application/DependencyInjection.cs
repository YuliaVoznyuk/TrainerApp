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
        // 🔹 Реєструємо всі сервісні інтерфейси (Application-level logic)
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<INutritionService, NutritionService>();

        // 🔹 Реєстрація use-case handler-ів (Auth)
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();

        // 🔹 Реєстрація use-case handler-ів (Client)
        services.AddScoped<GetClientTrainingsHandler>();
        
        services.AddScoped<CancelTrainingHandler>();
        services.AddScoped<UploadProgressPhotoHandler>();

        // 🔹 Реєструємо AuthService (Application-level orchestrator)
        services.AddScoped<IAuthService, AuthService>();

        // 🔹 Реєструємо FluentValidation валідатори
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}