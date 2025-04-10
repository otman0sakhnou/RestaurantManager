using System.ComponentModel.DataAnnotations;

namespace RestoManagement.Domain.DTOs;

public record RegisterDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "Please select your role")]
    public string Role { get; set; } = string.Empty;
}   