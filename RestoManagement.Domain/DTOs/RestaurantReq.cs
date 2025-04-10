using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RestoManagement.Domain.DTOs;

public record RestaurantReq
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    
    public string Adresse { get; set; } = string.Empty;
    
    public string Cuisine { get; set; } = string.Empty;
    public double Note { get; set; }
    
    public IFormFile? Image { get; set; }
};