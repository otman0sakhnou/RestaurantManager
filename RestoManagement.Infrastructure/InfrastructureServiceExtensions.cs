using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestoManagement.Domain.Contracts.IRepositories;
using RestoManagement.Domain.Models;
using RestoManagement.Infrastructure.Data;
using RestoManagement.Infrastructure.Repositories;

namespace RestoManagement.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Register DbContext with SQL Server and Identity
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        // Register Identity services
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Register repositories
        services.AddScoped<IRestaurantRepository, RestaurantRepository>(); 
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<AppDbSeeder>();
        return services;
    }
}