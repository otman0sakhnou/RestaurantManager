using Microsoft.AspNetCore.Identity;
using RestoManagement.Domain.Models;

namespace RestoManagement.Domain.Contracts.IRepositories;

public interface IAuthRepository
{
    Task<IdentityResult> RegisterUserAsync(string email, string password,string role);
    Task<SignInResult> CheckPasswordAsync(string email, string password);
    Task<AppUser?> FindByEmailAsync(string email);
    Task<IList<string>> GetUserRolesAsync(AppUser user);
    Task AddUserToRoleAsync(AppUser user, string role);
}