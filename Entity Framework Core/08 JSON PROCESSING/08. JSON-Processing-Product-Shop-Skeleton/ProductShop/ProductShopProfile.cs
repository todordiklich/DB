using AutoMapper;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDTO, User>();

            CreateMap<ProductDTO, Product>();

            CreateMap<CategoryDTO, Category>();

            CreateMap<CategotyProductDTO, CategoryProduct>();
        }
    }
}
