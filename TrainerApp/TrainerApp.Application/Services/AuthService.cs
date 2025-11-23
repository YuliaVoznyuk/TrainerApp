using Microsoft.AspNetCore.Identity;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Application.UseCases.Auth;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly RegisterUserHandler _registerHandler;
    private readonly LoginUserHandler _loginHandler;


    public AuthService(
        RegisterUserHandler registerHandler,
        LoginUserHandler loginHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        await _registerHandler.HandleAsync(dto);
    }


    public async Task<string> LoginAsync(LoginDto dto)
    {
        return await _loginHandler.HandleAsync(dto);
    }
}