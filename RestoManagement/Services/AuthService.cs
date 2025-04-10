using System.Net.Http.Json;
using System.Text.Json;
using RestoManagement.Contracts;
using RestoManagement.Domain.DTOs;
using RestoManagement.Domain.Helpers;

namespace RestoManagement.Services;


public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
    }
 
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponse>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    
        throw new ApplicationException(error?.Message ?? "Une erreur est survenue.");
    }

}