using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddCategoryAsync(CategoryModel model)
        {
            model.CategoryId = 0; // tránh lỗi tạo key auto
            if (model.Description == "string")
            {
                model.Description = null; // tránh lưu vào là từ string
            }

            var newCategory = _mapper.Map<Category>(model);

            var existingCategory = await _context.Categories
                                                 .FirstOrDefaultAsync(e => e.CategoryName == newCategory.CategoryName);

            if (existingCategory != null)
            {
                throw new ArgumentException("Tên danh mục đã tồn tại!");
            }

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return newCategory.CategoryId;
        }

        public async Task<CategoryModel> CheckNullModelAsync(int id, CategoryModel model)
        {
            var category = await GetCategoryByIdAsync(id);

            if (model.CategoryName == "string" || model.CategoryName == null)
            {
                model.CategoryName = category.CategoryName;
            }

            if (model.Description == "string" || model.Description == null)
            {
                model.Description = category.Description;
            }

            return model;
        }

        // Xóa danh mục
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products) // Bao gồm sản phẩm liên quan
                .FirstOrDefaultAsync(c => c.CategoryId == id); // Tìm danh mục theo ID

            if (category == null)
            {
                throw new KeyNotFoundException("Không có danh mục tồn tại");
            }

            // Kiểm tra xem có sản phẩm nào thuộc danh mục này hay không
            if (category.Products != null && category.Products.Any())
            {
                // Nếu có sản phẩm, ném ra lỗi và có thể trả về danh sách sản phẩm
                throw new InvalidOperationException($"Không thể xóa danh mục vì có sản phẩm liên kết: {string.Join(", ", category.Products.Select(p => p.ProductID))}.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }



        // Lấy tất cả danh mục
        public async Task<PagedResult<CategoryModel>> GetAllCategoryAsync(int page, int pageSize)
        {
            var totalCategories = await _context.Categories.CountAsync(); // Đếm tổng số danh mục
            var categories = await _context.Categories
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            var categoryModels = _mapper.Map<List<CategoryModel>>(categories);

            return new PagedResult<CategoryModel>
            {
                Items = categoryModels,
                TotalCount = totalCategories,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        // Tìm kiếm danh mục
        public async Task<PagedResult<CategoryModel>> GetCategoriesByNameAsync(string name, int page, int pageSize)
        {
            // Đếm tổng số danh mục có tên chứa ký tự 'name'
            var totalCategories = await _context.Categories
                .Where(c => c.CategoryName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .CountAsync();

            // Lấy danh mục theo tên với phân trang
            var categories = await _context.Categories
                .Where(c => c.CategoryName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            var categoryModels = _mapper.Map<List<CategoryModel>>(categories);

            return new PagedResult<CategoryModel>
            {
                Items = categoryModels,
                TotalCount = totalCategories,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        // Lấy danh mục theo Id
        public async Task<CategoryModel> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return null;
            }
            return _mapper.Map<CategoryModel>(category);
        }

        // Cập nhật lại danh mục
        public async Task UpdateCategoryAsync(int id, CategoryModel model)
        {
            model =  await CheckNullModelAsync(id, model);

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Không có Id danh mục");
            }

            model.CategoryId = id; // Gắn id truyền vào form nếu không nhập

            _mapper.Map(model, category);

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
