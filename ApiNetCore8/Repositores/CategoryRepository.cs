  using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            model.CategoryId = 0; // Tránh lỗi tạo key auto
            if (model.Description == "string")
            {
                model.Description = null; // Tránh lưu vào là từ "string"
            }

            // Ánh xạ từ model sang entity Category
            var newCategory = _mapper.Map<Category>(model);

            // Kiểm tra xem danh mục có bị trùng tên hay không
            var existingCategory = await _context.Categories
                                                 .FirstOrDefaultAsync(e => e.CategoryName == newCategory.CategoryName);

            if (existingCategory != null)
            {
                throw new ArgumentException("Tên danh mục đã tồn tại!");
            }

            // Thêm category vào cơ sở dữ liệu
            await _context.Categories.AddAsync(newCategory);

            // Kiểm tra và thêm các Supplier vào danh mục nếu có
            if (model.Supplier != null && model.Supplier.Any())
            {
                // Lấy các nhà cung cấp từ SupplierId trong model
                var suppliers = await _context.Suppliers
                                              .Where(s => model.Supplier.Select(s => s.SupplierId).Contains(s.SupplierId))
                                              .ToListAsync();

                if (newCategory.Suppliers == null)
                {
                    newCategory.Suppliers = new List<Supplier>();
                }

                // Thêm các Supplier vào danh mục
                foreach (var supplier in suppliers)
                {
                    newCategory.Suppliers.Add(supplier);
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return newCategory.CategoryId;
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
                .Include(c => c.Suppliers) // Bao gồm Suppliers
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            // Ánh xạ thủ công từ Category sang CategoryModel
            var categoryModels = categories.Select(c => new CategoryModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Supplier = c.Suppliers.Select(s => new SupplierInfo
                {
                    SupplierId = s.SupplierId, // Lấy SupplierId
                    SupplierName = s.SupplierName // Lấy SupplierName
                }).ToList() // Tạo danh sách các SupplierInfo
            }).ToList();

            return new PagedResult<CategoryModel>
            {
                Items = categoryModels,
                TotalCount = totalCategories,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        // Tìm kiếm danh mục
        public async Task<PagedResult<CategoryModel>> FindCategoriesAsync(string name, int page, int pageSize)
        {
            // Đếm tổng số danh mục có tên chứa ký tự 'name'
            var totalCategories = await _context.Categories
                .Where(c => c.CategoryName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .CountAsync();

            // Lấy danh mục theo tên với phân trang
            var categories = await _context.Categories
                .Where(c => c.CategoryName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .Include(c => c.Suppliers) // Bao gồm Suppliers nếu cần thiết
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            // Ánh xạ từ entities sang DTO (CategoryModel)
            var categoryModels = categories.Select(c => new CategoryModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Supplier = c.Suppliers?.Select(s => new SupplierInfo
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName
                }).ToList()
            }).ToList();

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
                throw new KeyNotFoundException("Không có danh mục tồn tại");
            }
            return _mapper.Map<CategoryModel>(category);
        }

        public async Task UpdateCategoryAsync(int id, CategoryModel model)
        {
            // Tìm danh mục theo ID
            var category = await _context.Categories.Include(c => c.Suppliers).FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                throw new KeyNotFoundException("Không có Id danh mục");
            }

            // Cập nhật các trường thông tin danh mục
            category.CategoryName = model.CategoryName;
            category.Description = model.Description;

            // Kiểm tra và cập nhật các nhà cung cấp nếu có thay đổi
            if (model.Supplier != null && model.Supplier.Any())
            {
                // Lấy danh sách các SupplierId trong model
                var supplierIds = model.Supplier.Select(s => s.SupplierId).ToList();

                // Tìm các nhà cung cấp từ cơ sở dữ liệu
                var suppliers = await _context.Suppliers
                                              .Where(s => supplierIds.Contains(s.SupplierId))
                                              .ToListAsync();

                // Xóa các nhà cung cấp không còn liên kết với danh mục này
                category.Suppliers.Clear();

                // Thêm lại các nhà cung cấp mới vào danh mục
                foreach (var supplier in suppliers)
                {
                    category.Suppliers.Add(supplier);
                }
            }

            // Cập nhật thông tin danh mục trong cơ sở dữ liệu
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
