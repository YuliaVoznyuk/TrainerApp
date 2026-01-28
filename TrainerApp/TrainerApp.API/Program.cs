using System.Text;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrainerApp.API.Context;
using TrainerApp.API.Middleware;
using TrainerApp.Application;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.Interfaces.FileStorage;
using TrainerApp.Application.Services;
using TrainerApp.Application.Validators;
using TrainerApp.Domain.Entities;
using TrainerApp.Infrastructure;
using TrainerApp.Infrastructure.Jwt;
using TrainerApp.Infrastructure.Persistence;
using TrainerApp.Infrastructure.Seed;
using TrainerApp.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(config => 
        config.RegisterValidatorsFromAssemblyContaining<CreateSlotDtoValidator>());

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("http://localhost:5049")  
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddMapster();

builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AppUserSeeder>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITrainerContext, TrainerContext>();
builder.Services.AddScoped<IPhotoStorage, S3PhotoStorage>();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseCors("AllowBlazor");
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var seeder = services.GetRequiredService<AppUserSeeder>();

    db.Database.Migrate();           
    await seeder.SeedAsync();        
}

app.Run();