using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using AutoMapper;
using Microsoft.CodeAnalysis;
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
            model.ProductID = 0;
            if (model.Description == null || model.Description == "string")
            {
                model.Description = null;
            }

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

        public Task<ProductModel> CheckNullProductModel(int id, UpdateProductModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ProductModel>> FindProductsAsync(String name, int page, int pageSize)
        {
            // Đếm tổng số danh mục có tên chứa ký tự 'name'
            var totalProducts = await _context.Products
                .Where(p => p.ProductName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .CountAsync();

            // Lấy danh mục theo tên với phân trang
            var products = await _context.Products
                .Where(p => p.ProductName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            var ProductModels = _mapper.Map<List<ProductModel>>(products);

            return new PagedResult<ProductModel>
            {
                Items = ProductModels,
                TotalCount = totalProducts,
                PageSize = pageSize,
                CurrentPage = page
            };
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

            if (product != null)
            {
                return _mapper.Map<ProductModel>(product);
            }
            
            throw new NotImplementedException("Không tìm thấy sản phẩm");
        }



        public async Task UpdateProductAsync(int id, UpdateProductModel model)
        {
            var existingProduct = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Không tìm thấy sản phẩm");
            }

            // Cập nhật các thuộc tính từ model
            _mapper.Map(model, existingProduct);

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
        }
    }
    }

