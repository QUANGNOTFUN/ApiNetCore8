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
            var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == model.CategoryID);

            if (category == null)
            {
                throw new ArgumentException("Không có Id danh mục sản phẩm");
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
        public async Task<PagedResult<ProductModel>> GetAllProductsAsync(int page, int pageSize)
        {
            var totalProducts = await _context.Products.CountAsync(); // Đếm tổng số sản phẩm
            var products = await _context.Products
                .Skip((page - 1) * pageSize) // Bỏ qua các sản phẩm ở các trang trước
                .Take(pageSize) // Lấy số sản phẩm trong trang hiện tại
                .ToListAsync();

            var productModels = _mapper.Map<List<ProductModel>>(products);

            return new PagedResult<ProductModel>
            {
                Items = productModels,
                TotalCount = totalProducts,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<PagedResult<ProductModel>> GetLowStockProductsAsync(int page, int pageSize)
        {
            // Lấy danh sách sản phẩm có mức tồn kho dưới mức cần nhập
            var totalLowStockProducts = await _context.Products
                .Where(p => p.ReorderLevel > p.StockQuantity)
                .CountAsync(); // Đếm tổng số sản phẩm có tồn kho thấp

            var lowStockProducts = await _context.Products
                .Where(p => p.ReorderLevel > p.StockQuantity) // Điều kiện sản phẩm có tồn kho thấp
                .Skip((page - 1) * pageSize) // Bỏ qua các sản phẩm ở các trang trước
                .Take(pageSize) // Lấy số sản phẩm trong trang hiện tại
                .ToListAsync();

            var lowStockProductModels = _mapper.Map<List<ProductModel>>(lowStockProducts);

            return new PagedResult<ProductModel>
            {
                Items = lowStockProductModels,
                TotalCount = totalLowStockProducts,
                PageSize = pageSize,
                CurrentPage = page
            };
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
