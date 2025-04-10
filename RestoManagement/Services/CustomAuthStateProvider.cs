using System.Diagnostics;
using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using Microsoft.JSInterop;
using RestoManagement.Domain.DTOs;

namespace RestoManagement.Services;
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var email = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userEmail");
        var rolesJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userRoles");
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email ?? "Unknown User")
        };
        
        if (!string.IsNullOrEmpty(rolesJson))
        {
            try
            {
                var roles = JsonSerializer.Deserialize<string[]>(rolesJson);
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyUserAuthentication(AuthResponseDto response)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, response.Email)
        };
        
        if (response.Roles != null)
        {
            foreach (var role in response.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
}