using AutoMapper;
using Data.Entities;
using Model.Campaign;
using Model.Order;
using Model.Product;

namespace Service
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();

            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();

            CreateMap<Campaign, CampaignDto>();
            CreateMap<CreateCampaignDto, Campaign>();
        }
    }
}
