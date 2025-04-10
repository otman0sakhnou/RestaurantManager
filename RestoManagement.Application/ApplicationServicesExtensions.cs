
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RestoManagement.Application.RequestHelpers.Validations;
using RestoManagement.Application.Services;
using RestoManagement.Domain.Contracts.IServices;

namespace RestoManagement.Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register FluentValidation
        services.AddValidatorsFromAssemblyContaining<RestaurantReqValidation>();
        //register autoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        //Register Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRestaurantService, RestaurantService>();
        
        return services;
    }
}