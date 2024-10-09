using ShoppingWebApi.DTOs;
using ShoppingWebApi.Models;
namespace ShoppingWebApi.Profile;
using AutoMapper; 

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
    }
}