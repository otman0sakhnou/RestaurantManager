using RestoManagement.Domain.Models;

namespace RestoManagement.Domain.Contracts.IRepositories;

public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(Guid id);
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<IEnumerable<Restaurant>> GetByCuisineAsync(string cuisine);
    Task AddAsync(Restaurant restaurant);
    Task UpdateAsync(Restaurant restaurant);
    Task DeleteAsync(Guid id);
}