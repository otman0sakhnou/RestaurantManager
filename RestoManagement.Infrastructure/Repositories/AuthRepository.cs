using Microsoft.AspNetCore.Identity;
using RestoManagement.Domain.Contracts.IRepositories;
using RestoManagement.Domain.Models;

namespace RestoManagement.Infrastructure.Repositories;

public class AuthRepository :IAuthRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;


    public AuthRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

   public async Task<IdentityResult> RegisterUserAsync(string email, string password, string role)
{
    if (string.IsNullOrEmpty(email))
    {
        throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
    }
    if (string.IsNullOrEmpty(password))
    {
        throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
    }
    if (string.IsNullOrEmpty(role))
    {
        throw new ArgumentNullException(nameof(role), "Role cannot be null or empty");
    }

    var user = new AppUser { Email = email, UserName = email };

    
    var result = await _userManager.CreateAsync(user, password);
    if (!result.Succeeded)
    {
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Error: {error.Description}");  
        }
        return result;
    }
    
    var roleExists = await _roleManager.RoleExistsAsync(role);
    if (!roleExists)
    {
        var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
        if (!roleResult.Succeeded)
        {
            foreach (var error in roleResult.Errors)
            {
                Console.WriteLine($"Role Creation Error: {error.Description}");  
            }
            return roleResult;
        }
    }

    // Assign the role to the user
    var addToRoleResult = await _userManager.AddToRoleAsync(user, role);
    if (!addToRoleResult.Succeeded)
    {
        foreach (var error in addToRoleResult.Errors)
        {
            Console.WriteLine($"Role Assignment Error: {error.Description}");  
        }
        return addToRoleResult; 
    }

    return result;
}



    public async Task<SignInResult> CheckPasswordAsync(string email, string password)
    {
        var user = await FindByEmailAsync(email);
        return user != null
            ? await _signInManager.CheckPasswordSignInAsync(user, password, false)
            : SignInResult.Failed;
    }

    public Task<AppUser?> FindByEmailAsync(string email) =>
        _userManager.FindByEmailAsync(email);

    public Task<IList<string>> GetUserRolesAsync(AppUser user) =>
        _userManager.GetRolesAsync(user);

    public Task AddUserToRoleAsync(AppUser user, string role) =>
        _userManager.AddToRoleAsync(user, role);
}
