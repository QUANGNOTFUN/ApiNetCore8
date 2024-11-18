  using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        // Kiểm tra categoryudatemodel thuộc tính nào null
        public async Task<CategoryUpdateModel> CheckNullCategoryAsync(int id, CategoryUpdateModel model)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);

            if (string.IsNullOrWhiteSpace(model.CategoryName) || model.CategoryName == "string")
            {
                model.CategoryName = category.CategoryName;
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.CategoryName == "string")
            {
                model.Description = category.Description;
            }

            return model;
        }

        // Cập nhật category
        public async Task UpdateCategoryAsync(int id, CategoryUpdateModel model)
        {
            // Tìm danh mục theo ID, bao gồm cả danh sách nhà cung cấp
            var category = await _context.Categories
                                         .Include(c => c.Suppliers) // Gắn danh sách nhà cung cấp
                                         .FirstOrDefaultAsync(c => c.CategoryId == id);

            // Kiểm tra và thay thế các giá trị null trong model
            var checkModel = await CheckNullCategoryAsync(id, model);
            category.CategoryName = checkModel.CategoryName;
            category.Description = checkModel.Description;

            if (checkModel.SupplierIdNew != null && checkModel.SupplierIdNew.Any())
            {
                // Loại bỏ các ID bằng 0
                checkModel.SupplierIdNew.RemoveAll(id => id == 0);

                if (!checkModel.SupplierIdNew.Any())
                {
                    throw new InvalidOperationException("Danh sách ID nhà cung cấp chỉ chứa giá trị không hợp lệ (0).");
                }

                // Xóa toàn bộ danh sách nhà cung cấp cũ
                category.Suppliers.Clear();

                // Lấy danh sách nhà cung cấp từ cơ sở dữ liệu dựa trên danh sách ID
                var newSuppliers = await _context.Suppliers
                    .Where(s => checkModel.SupplierIdNew.Contains(s.SupplierId))
                    .ToListAsync();

                // Kiểm tra số lượng nhà cung cấp tìm được có khớp với danh sách ID
                if (newSuppliers.Count != checkModel.SupplierIdNew.Count)
                {
                    throw new InvalidOperationException("Không tìm thấy đầy đủ nhà cung cấp theo danh sách ID được cung cấp.");
                }

                // Thêm tất cả nhà cung cấp mới vào danh sách
                foreach (var newSupplier in newSuppliers)
                {
                    if (!category.Suppliers.Contains(newSupplier))
                    {
                        category.Suppliers.Add(newSupplier);
                    }
                }
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

    }
}
