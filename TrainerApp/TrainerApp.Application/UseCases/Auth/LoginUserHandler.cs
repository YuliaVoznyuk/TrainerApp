using Microsoft.AspNetCore.Identity;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;

namespace TrainerApp.Application.UseCases.Auth;

public class LoginUserHandler
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginUserHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string> HandleAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
                   ?? throw new UnauthorizedAccessException("Invalid credentials.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var roles = await _userManager.GetRolesAsync(user);
        return _tokenGenerator.GenerateToken(user, roles);
    }
}