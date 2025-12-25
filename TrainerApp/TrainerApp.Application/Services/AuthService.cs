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
        try
        {
            await _registerHandler.HandleAsync(dto);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message); 
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message); 
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Помилка реєстрації: " + ex.Message);
        }
    }


    public async Task<string> LoginAsync(LoginDto dto)
    {
        return await _loginHandler.HandleAsync(dto);
    }
   
}