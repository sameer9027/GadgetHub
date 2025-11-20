using GadgetHub.Application.DTOs.Auth;
using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Application.DTOs.Products;
using GadgetHub.Domain.Entities;
using Mapster;

namespace GadgetHub.Application.Common.Mappings
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Product mappings
            config.NewConfig<Product, ProductDto>()
                  .Map(dest => dest.CategoryName, src => src.Category.Name);

            config.NewConfig<CreateProductDto, Product>();
            config.NewConfig<UpdateProductDto, Product>();
            config.NewConfig<RegisterRequestDto, User>();
            
            // Category mappings
            config.NewConfig<Category, CategoryDto>();
            config.NewConfig<CreateCategoryDto, Category>();
            config.NewConfig<UpdateCategoryDto, Category>();
        }
    }
}