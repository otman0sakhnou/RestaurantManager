using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestoManagement.Domain.Contracts.IRepositories;
using RestoManagement.Domain.Contracts.IServices;
using RestoManagement.Domain.DTOs;
using RestoManagement.Domain.Models;

namespace RestoManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepo;
    private readonly IConfiguration _config;

    public AuthService(IAuthRepository authRepo, IConfiguration config)
    {
        _authRepo = authRepo;
        _config = config;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var result = await _authRepo.RegisterUserAsync(dto.Email, dto.Password,dto.Role);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Échec de l'enregistrement: {errorMessages}");
        }

        var user = await _authRepo.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new Exception("Utilisateur introuvable après l'enregistrement.");
        }

        await _authRepo.AddUserToRoleAsync(user, dto.Role);

        var token = await GenerateJwtToken(user);
        var roles = await _authRepo.GetUserRolesAsync(user);

        return new AuthResponseDto(token, dto.Email, roles.ToList());
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _authRepo.FindByEmailAsync(dto.Email);
        if (user == null || !(await _authRepo.CheckPasswordAsync(dto.Email, dto.Password)).Succeeded)
        {
            throw new Exception("Identifiants invalides.");
        }

        var token = await GenerateJwtToken(user);
        var roles = await _authRepo.GetUserRolesAsync(user);

        return new AuthResponseDto(token, dto.Email, roles.ToList());
    }

    public async Task<string> GenerateJwtToken(AppUser user)
    {
        if (string.IsNullOrEmpty(_config["Jwt:SecretKey"]))
        {
            throw new ArgumentNullException("Jwt:SecretKey", "SecretKey is not configured properly.");
        }
        if (string.IsNullOrEmpty(_config["Jwt:Issuer"]))
        {
            throw new ArgumentNullException("Jwt:Issuer", "Issuer is not configured properly.");
        }
        if (string.IsNullOrEmpty(_config["Jwt:Audience"]))
        {
            throw new ArgumentNullException("Jwt:Audience", "Audience is not configured properly.");
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}