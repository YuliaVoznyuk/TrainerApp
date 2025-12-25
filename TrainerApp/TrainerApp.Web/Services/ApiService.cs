using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using TrainerApp.Web.Models;
using TrainerApp.Web.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
namespace TrainerApp.Web.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";

    public ApiService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddAuthorizationHeader()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }


    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var loginData = new { Email = email, Password = password };
        var response = await _http.PostAsJsonAsync("api/auth/login", loginData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await _localStorage.SetItemAsync(TokenKey, result.Token);
            }
            return result ?? new AuthResponse { Success = true };        }

        var error = await response.Content.ReadAsStringAsync();
        return new AuthResponse 
        { 
            Success = false, 
            Message = error.Contains("Message") ? error : "Невірний email або пароль"
        };    }

    public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", dto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponse>() ?? new AuthResponse { Success = true };
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return new AuthResponse { Success = false, Message = error };
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        _http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);
        return !string.IsNullOrEmpty(token);
    }
    
    public async Task<string?> GetUserRoleAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
        return roleClaim?.Value;
    }
   


   
}