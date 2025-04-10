using Microsoft.AspNetCore.Mvc;
using RestoManagement.Domain.Contracts.IServices;
using RestoManagement.Domain.DTOs;

namespace RestoManagement.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController :ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto model)
    {
        try
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
        }
        catch (Exception ex)  
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto model)
    {
        try
        {
            var result = await _authService.LoginAsync(model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
