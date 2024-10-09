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
        }
    }
}
