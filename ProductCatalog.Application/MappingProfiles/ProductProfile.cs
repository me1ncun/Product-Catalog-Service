using AutoMapper;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Core.Entities;

namespace ProductCatalog.Application.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductListDto>().ReverseMap();
    }
}