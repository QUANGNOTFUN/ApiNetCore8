﻿using ApiNetCore8.Data;
using ApiNetCore8.Models;
using AutoMapper;

namespace ApiNetCore8.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Category, CategoryData>().ReverseMap();
            CreateMap<Product, ProductData>().ReverseMap();
            CreateMap<Supplier, SupplierData>().ReverseMap();
        }
    }
}
