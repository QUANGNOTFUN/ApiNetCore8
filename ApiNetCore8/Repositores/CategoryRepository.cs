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

            foreach (var suppId in model.Supplier)
            {
                if (suppId.SupplierIdNew != 0)
                {
                    var existSupp = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == suppId.SupplierIdNew);
                    if (existSupp == null)
                    {
                        throw new KeyNotFoundException("Không có id nhà cung cấp để cập nhật");
                    }
                }
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

            // Cập nhật danh sách nhà cung cấp
            foreach (var supplierUpdate in checkModel.Supplier)
            {
                if (supplierUpdate.SupplierIdOld != 0 && supplierUpdate.SupplierIdNew != 0)
                {
                    // Trường hợp thay đổi từ SupplierIdOld sang SupplierIdNew
                    var oldSupplier = category.Suppliers.FirstOrDefault(s => s.SupplierId == supplierUpdate.SupplierIdOld);
                    if (oldSupplier != null)
                    {
                        oldSupplier.SupplierId = supplierUpdate.SupplierIdNew;
                    }
                }
                else if (supplierUpdate.SupplierIdNew != 0 && supplierUpdate.SupplierIdOld == 0)
                {
                    // Trường hợp thêm nhà cung cấp mới
                    var newSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierUpdate.SupplierIdNew);
                    if (newSupplier != null && !category.Suppliers.Contains(newSupplier))
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
