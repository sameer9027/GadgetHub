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

 
          
            // Category mappings
            config.NewConfig<Category, CategoryDto>();
         
        }
    }
}