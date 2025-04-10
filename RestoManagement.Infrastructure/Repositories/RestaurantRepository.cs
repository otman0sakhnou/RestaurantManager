using Microsoft.EntityFrameworkCore;
using RestoManagement.Domain.Contracts.IRepositories;
using RestoManagement.Domain.Models;
using RestoManagement.Infrastructure.Data;

namespace RestoManagement.Infrastructure.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly AppDbContext _context;

    public RestaurantRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Restaurant restaurant)
    {
        try
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Repository Error] Erreur lors de l'ajout du restaurant : {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var restaurant = await GetByIdAsync(id);

            if (restaurant == null)
                throw new KeyNotFoundException("Aucun restaurant trouvé avec cet identifiant.");

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Repository Error] Erreur lors de la suppression du restaurant : {ex.Message}");
            throw;
        }
    }

    public async Task<Restaurant?> GetByIdAsync(Guid id)
    {
        try
        {
            var restaurant = await _context.Restaurants.FindAsync(id);


            return restaurant;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Repository Error] Erreur lors de la récupération par ID : {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        try
        {

            return await _context.Restaurants.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[Repository Error] Erreur lors de la récupération de tous les restaurants : {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Restaurant>> GetByCuisineAsync(string cuisine)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cuisine))
                throw new ArgumentException("Le type de cuisine est requis pour la recherche.");

            return await _context.Restaurants
                .Where(r => r.Cuisine == cuisine)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Repository Error] Erreur lors de la recherche par type de cuisine : {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        try
        {
            if (restaurant == null)
                throw new ArgumentNullException(nameof(restaurant), "L'objet restaurant ne peut pas être nul.");


            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Repository Error] Erreur lors de la mise à jour du restaurant : {ex.Message}");
            throw;
        }
    }
}