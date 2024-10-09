using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Repositores
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(InventoryContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddProductAsync(ProductModel model)
        {
            var newProduct = _mapper.Map<Product>(model);
            var category = await _context.Categories
                                         .Include(c => c.Products) // Load products if not loaded
                                         .FirstOrDefaultAsync(c => c.CategoryId == model.CategoryID);

            if (category == null)
            {
                throw new ArgumentException("Invalid Category ID");
            }   

            newProduct.Category = category;

            // Lưu sản phẩm mới vào cơ sở dữ liệu
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return newProduct.ProductID;
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductModel>>(products);
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return null;
            }
            return _mapper.Map<ProductModel>(product);
        }

        public Task UpdateProductAsync(int id, ProductModel model)
        {
            throw new NotImplementedException();
        }
    }
}
