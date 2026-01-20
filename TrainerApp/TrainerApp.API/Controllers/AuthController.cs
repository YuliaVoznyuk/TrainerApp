using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TrainerApp.Application.DTOs;
using TrainerApp.Application.Interfaces;
using TrainerApp.Domain.Entities;
using TrainerApp.Application.DTOs;

namespace TrainerApp.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAndLoginAsync(dto);

        return Ok(new
        {
            Success = true,
            result.Token,
            result.Role,
            Message = "Реєстрація та вхід успішні!"
        });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);

        return Ok(new
        {
            Success = true,
            Token = token,
            Message = "Вхід успішний!"
        });
    }

    }
