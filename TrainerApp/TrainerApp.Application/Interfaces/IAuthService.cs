using TrainerApp.Application.DTOs;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAndLoginAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);


}