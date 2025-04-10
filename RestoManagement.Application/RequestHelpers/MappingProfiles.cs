using AutoMapper;
using RestoManagement.Domain.DTOs;
using RestoManagement.Domain.Models;

namespace RestoManagement.Application.RequestHelpers;

public class MappingProfiles :Profile
{
    public MappingProfiles()
    {
        CreateMap<Restaurant, RestaurantRes>();
        CreateMap<RestaurantReq, Restaurant>();
    }
}