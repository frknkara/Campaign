using AutoMapper;
using Data.Entities;
using Model.Product;

namespace Service
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
        }
    }
}
