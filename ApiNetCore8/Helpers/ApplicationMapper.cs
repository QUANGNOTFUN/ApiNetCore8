using ApiNetCore8.Data;
using ApiNetCore8.Models;
using AutoMapper;

namespace ApiNetCore8.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Supplier, SupplierModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<Supplier, SupplierModel>()
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Categories.Select(c => c.CategoryId).FirstOrDefault()));
            CreateMap<OrderModel, Order>()
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        }
    }
}
