namespace RestoManagement.Infrastructure.Data;

public class AppDbSeeder
{
    private readonly AppDbContext _context;

    public AppDbSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Restaurants.Any())
        {
            var data = RestaurantSeeder.Seed();
            _context.Restaurants.AddRange(data);
            _context.SaveChanges();
        }
    } 
}