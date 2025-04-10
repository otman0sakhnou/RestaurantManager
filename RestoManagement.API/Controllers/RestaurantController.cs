using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestoManagement.Domain.Contracts.IServices;
using RestoManagement.Domain.DTOs;
using Microsoft.Extensions.Logging;

namespace RestoManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    private readonly ILogger<RestaurantsController> _logger;

    public RestaurantsController(IRestaurantService restaurantService, ILogger<RestaurantsController> logger)
    {
        _restaurantService = restaurantService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _restaurantService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _restaurantService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("cuisine/{cuisine}")]
    public async Task<IActionResult> GetByCuisine(string cuisine)
    {
        var result = await _restaurantService.GetByCuisineAsync(cuisine);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] RestaurantReq restaurantReq)
    {
        await _restaurantService.AddAsync(restaurantReq);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] RestaurantReq restaurantReq)
    {
        await _restaurantService.UpdateAsync(restaurantReq);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _restaurantService.DeleteAsync(id);
        return NoContent();
    }
}