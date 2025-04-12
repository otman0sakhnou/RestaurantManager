using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using RestoManagement.Domain.DTOs;

namespace RestoManagement.BlazorApp.Services;

public class RestaurantService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _jsRuntime;
    private const string BaseUrl = "api/restaurants";

    public RestaurantService(HttpClient http, IJSRuntime jsRuntime)
    {
        _http = http;
        _jsRuntime = jsRuntime;
    }

    private async Task AddAuthorizationHeaderAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IEnumerable<RestaurantRes>?> GetAllAsync()
    {
        await AddAuthorizationHeaderAsync();
        return await _http.GetFromJsonAsync<IEnumerable<RestaurantRes>>(BaseUrl);
    }

    public async Task<RestaurantRes?> GetByIdAsync(Guid id)
    {
        await AddAuthorizationHeaderAsync();
        return await _http.GetFromJsonAsync<RestaurantRes>($"{BaseUrl}/{id}");
    }

    public async Task<IEnumerable<RestaurantRes>?> GetByCuisineAsync(string cuisine)
    {
        await AddAuthorizationHeaderAsync();
        return await _http.GetFromJsonAsync<IEnumerable<RestaurantRes>>($"{BaseUrl}/cuisine/{cuisine}");
    }

    public async Task<bool> AddAsync(RestaurantReq restaurantReq)
    {
        await AddAuthorizationHeaderAsync();
        var content = ToMultipartContent(restaurantReq);
        var response = await _http.PostAsync(BaseUrl, content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(RestaurantReq restaurantReq)
    {
        await AddAuthorizationHeaderAsync();
        var content = ToMultipartContent(restaurantReq);
        var response = await _http.PutAsync(BaseUrl, content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _http.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }

  
    public async Task<string> GetImageUrlAsync(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return string.Empty;
        
        if (imagePath.StartsWith("http://") || imagePath.StartsWith("https://"))
            return imagePath;
        
        var baseUrl = _http.BaseAddress.ToString();
    
        if (baseUrl.EndsWith("/"))
            baseUrl = baseUrl.TrimEnd('/');
        
        if (imagePath.StartsWith("/"))
            imagePath = imagePath.Substring(1);
        
        return $"{baseUrl}/{imagePath}";
    }

    private MultipartFormDataContent ToMultipartContent(RestaurantReq req)
    {
        var content = new MultipartFormDataContent();
    
        content.Add(new StringContent(req.Id.ToString()), "Id");
        content.Add(new StringContent(req.Nom), "Nom");
        content.Add(new StringContent(req.Adresse), "Adresse");
        content.Add(new StringContent(req.Cuisine), "Cuisine");
        content.Add(new StringContent(req.Note.ToString()), "Note");

        if (req.Image != null)
        {
            var fileContent = new StreamContent(req.Image.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(req.Image.ContentType);
            content.Add(fileContent, "Image", req.Image.Name);
        }

        return content;
    }
}
