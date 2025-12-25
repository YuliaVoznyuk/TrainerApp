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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await _authService.RegisterAsync(dto);
            var token = await _authService.LoginAsync(new LoginDto 
            { 
                Email = dto.Email, 
                Password = dto.Password 
            });

            return Ok(new 
            { 
                Success = true, 
                Token = token,
                Role = dto.Role, 
                Message = "Реєстрація та вхід успішні!"
            });        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch
        {
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Success = false, Message = "Невірні дані" });

        try
        {
            var token = await _authService.LoginAsync(dto);
            

            return Ok(new 
            { 
                Success = true,
                Token = token,
                Message = "Вхід успішний!"
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { Success = false, Message = "Невірний email або пароль" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Success = false, Message = "Внутрішня помилка сервера" });
        }
    }
    }
