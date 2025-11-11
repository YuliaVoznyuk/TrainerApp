using System.Reflection;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.Extensions.DependencyInjection;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Services;

namespace TrainerApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Реєструємо всі сервіси бізнес-логіки
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<INutritionService, NutritionService>();

        // Реєструємо FluentValidation автоматично
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}