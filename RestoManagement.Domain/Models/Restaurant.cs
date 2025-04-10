namespace RestoManagement.Domain.Models;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Cuisine { get; set; } = string.Empty;
    public double Note { get; set; }
    public string? ImagePath { get; set; }
}