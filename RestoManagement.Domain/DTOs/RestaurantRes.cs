namespace RestoManagement.Domain.DTOs;

public record RestaurantRes
{
    public Guid Id { get; init; }
    public string Nom { get; init; } = string.Empty;
    public string Adresse { get; init; } = string.Empty;
    public string Cuisine { get; init; } = string.Empty;
    public double Note { get; init; }
    public string? ImagePath { get; init; }
    
}