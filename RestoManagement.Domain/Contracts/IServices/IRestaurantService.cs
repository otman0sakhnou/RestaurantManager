using Microsoft.AspNetCore.Http;
using RestoManagement.Domain.DTOs;

namespace RestoManagement.Domain.Contracts.IServices;

public interface IRestaurantService
{
    Task<RestaurantRes?> GetByIdAsync(Guid id);
    Task<IEnumerable<RestaurantRes>> GetAllAsync();
    Task<IEnumerable<RestaurantRes>> GetByCuisineAsync(string cuisine);
    Task AddAsync(RestaurantReq restaurant);
    Task UpdateAsync(RestaurantReq restaurant);
    Task DeleteAsync(Guid id);
}