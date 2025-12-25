using TrainerApp.Application.DTOs;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);

}