using RestoManagement.Domain.Models;

namespace RestoManagement.Infrastructure.Data;

public static class RestaurantSeeder
{
    public static List<Restaurant> Seed()
    {
        return new List<Restaurant>
        {
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Nom = "Le Gourmet",
                Adresse = "123 Rue de Paris, Paris",
                Cuisine = "Française",
                Note = 4.7,
                ImagePath =
                    "https://images.pexels.com/photos/3887985/pexels-photo-3887985.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Nom = "La Trattoria",
                Adresse = "456 Via Roma, Rome",
                Cuisine = "Italienne",
                Note = 4.5,
                ImagePath =
                    "https://images.pexels.com/photos/239975/pexels-photo-239975.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Nom = "Sushi Zen",
                Adresse = "789 Tokyo St, Tokyo",
                Cuisine = "Japonaise",
                Note = 4.8,
                ImagePath =
                    "https://images.pexels.com/photos/1383776/pexels-photo-1383776.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Nom = "Spicy Dragon",
                Adresse = "321 Nanjing Rd, Shanghai",
                Cuisine = "Chinoise",
                Note = 4.6,
                ImagePath =
                    "https://images.pexels.com/photos/3887985/pexels-photo-3887985.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Nom = "El Mariachi",
                Adresse = "654 Calle de Mexico, Mexico City",
                Cuisine = "Mexicaine",
                Note = 4.4,
                ImagePath =
                    "https://images.pexels.com/photos/239975/pexels-photo-239975.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Nom = "Burger Haven",
                Adresse = "111 Main St, New York",
                Cuisine = "Américaine",
                Note = 4.2,
                ImagePath =
                    "https://images.pexels.com/photos/1383776/pexels-photo-1383776.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            }
        };
    }
}