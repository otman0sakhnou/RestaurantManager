using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RestoManagement.Domain.Models; // Assuming AppUser is in this namespace

namespace RestoManagement.Infrastructure.Data;

public static class AuthSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // 1. Create roles
        string[] roles = { "Admin", "Manager", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role)); 
        }

        // 2. Create default admin user
        var adminEmail = "admin@resto.com";
        var adminPassword = "Admin@123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new AppUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}