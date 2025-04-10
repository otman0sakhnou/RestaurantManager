using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RestoManagement;
using RestoManagement.BlazorApp.Services;
using RestoManagement.Contracts;
using RestoManagement.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Explicitly set the base URL of your API here
builder.Services.AddScoped(sp => 
    new HttpClient 
    { 
        BaseAddress = new Uri("http://localhost:5084") 
    });

builder.Services.AddOptions();

builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();