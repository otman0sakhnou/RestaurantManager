using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestoManagement.Domain.Contracts.IRepositories;
using RestoManagement.Domain.Contracts.IServices;
using RestoManagement.Domain.DTOs;
using RestoManagement.Domain.Models;

namespace RestoManagement.Application.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _repository;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IValidator<RestaurantReq> _validator;
    private readonly IMapper _mapper;
    private readonly string _imageUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

    public RestaurantService(IRestaurantRepository repository,
        ILogger<RestaurantService> logger,
        IValidator<RestaurantReq> validator,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _mapper = mapper;
    }

    // Method to add a restaurant
    public async Task AddAsync(RestaurantReq restaurantDtoReq)
    {

        var validationResult = await _validator.ValidateAsync(restaurantDtoReq);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Échec de la validation pour le restaurant : {Nom}", restaurantDtoReq.Nom);
            throw new ValidationException(validationResult.Errors);
        }

        try
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantDtoReq);

            if (restaurantDtoReq.Image != null)
            {
                var imagePath = await UploadImageAsync(restaurantDtoReq.Image);
                restaurant.ImagePath = imagePath;
            }

            _logger.LogInformation("Ajout du restaurant : {Nom}", restaurant.Nom);
            await _repository.AddAsync(restaurant);
            _logger.LogInformation("Restaurant '{Nom}' ajouté avec succès.", restaurant.Nom);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'ajout du restaurant.");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Suppression du restaurant ID : {Id}", id);
        await _repository.DeleteAsync(id);
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_imageUploadPath, fileName);

            Directory.CreateDirectory(_imageUploadPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            _logger.LogInformation("Image téléchargée avec succès : {FileName}", fileName);

            return Path.Combine("images", fileName).Replace("\\", "/");
        }

        _logger.LogWarning("Aucune image à uploader");
        return string.Empty;
    }

    public async Task<IEnumerable<RestaurantRes>> GetAllAsync()
    {
        var restaurants = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RestaurantRes>>(restaurants);
    }

    public async Task<RestaurantRes?> GetByIdAsync(Guid id)
    {
        var restaurant = await _repository.GetByIdAsync(id);
        return _mapper.Map<RestaurantRes>(restaurant);
    }

    public async Task<IEnumerable<RestaurantRes>> GetByCuisineAsync(string cuisine)
    {
        var restaurant = await _repository.GetByCuisineAsync(cuisine);
        return _mapper.Map<IEnumerable<RestaurantRes>>(restaurant);
    }

    public async Task UpdateAsync(RestaurantReq restaurantReq)
    {
        var validationResult = await _validator.ValidateAsync(restaurantReq);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Échec de la validation pour le restaurant : {Nom}", restaurantReq.Nom);
            throw new ValidationException(validationResult.Errors);
        }

        try
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantReq);

            if (restaurantReq.Image != null)
            {
                var imagePath = await UploadImageAsync(restaurantReq.Image);
                restaurant.ImagePath = imagePath;
            }
            else
            {
                var existingRestaurant = await _repository.GetByIdAsync(restaurantReq.Id);
                if (existingRestaurant != null)
                {
                    restaurant.ImagePath = existingRestaurant.ImagePath;
                }
            }

            _logger.LogInformation("Mise à jour du restaurant ID : {Id}", restaurant.Id);
            await _repository.UpdateAsync(restaurant);
            _logger.LogInformation("Restaurant ID '{Id}' mis à jour avec succès.", restaurant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour du restaurant.");
            throw;
        }
    }
}