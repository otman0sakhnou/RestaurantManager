namespace RestoManagement.Domain.DTOs;

public record AuthResponseDto(string Token, string Email, List<string> Roles);