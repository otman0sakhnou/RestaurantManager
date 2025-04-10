using RestoManagement.Domain.DTOs;

namespace RestoManagement.Domain.Contracts.IServices;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}