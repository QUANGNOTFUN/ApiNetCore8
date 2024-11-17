using ApiNetCore8.Data;
using ApiNetCore8.Models;
using AutoMapper;

namespace ApiNetCore8.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // category
            CreateMap<Category, CategoryModel>().ReverseMap();

            // product
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Product, UpdateProductModel>().ReverseMap();

            // supplier
            CreateMap<Supplier, SupplierModel>().ReverseMap();

            // orderdetail
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();

            // order
            CreateMap<Order, OrderModel>().ReverseMap();
        }
    }
}
