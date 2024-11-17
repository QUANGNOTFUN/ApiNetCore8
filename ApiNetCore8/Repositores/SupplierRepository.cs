using ApiNetCore8.Data;
using ApiNetCore8.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace ApiNetCore8.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public SupplierRepository(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Thêm mới nhà cung cấp
        public async Task<int> AddSupplierAsync(SupplierModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Thông tin nhà cung cấp không được để trống");
            }

            model.SupplierId = 0; // Đảm bảo ID là auto-increment
            var newSupplier = _mapper.Map<Supplier>(model);

            await _context.Suppliers.AddAsync(newSupplier);
            await _context.SaveChangesAsync();

            return newSupplier.SupplierId;
        }

        // Lấy tất cả nhà cung cấp với phân trang
        public async Task<PagedResult<SupplierModel>> GetAllSuppliersAsync(int page, int pageSize)
        {
            var totalSuppliers = await _context.Suppliers.CountAsync(); // Đếm tổng số nhà cung cấp
            var suppliers = await _context.Suppliers
                .Include(s => s.Categories) // Bao gồm các danh mục liên quan đến mỗi nhà cung cấp
                .Skip((page - 1) * pageSize) // Bỏ qua các nhà cung cấp ở các trang trước
                .Take(pageSize) // Lấy số nhà cung cấp trong trang hiện tại
                .ToListAsync();

            // Ánh xạ từ Supplier sang SupplierModel
            var supplierModels = _mapper.Map<List<SupplierModel>>(suppliers);

            return new PagedResult<SupplierModel>
            {
                Items = supplierModels,
                TotalCount = totalSuppliers,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        // Lấy nhà cung cấp theo ID
        public async Task<SupplierModel> GetSupplierByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID phải lớn hơn 0.");
            }

            var supplier = await _context.Suppliers.SingleOrDefaultAsync(c => c.SupplierId == id);

            if (supplier == null)
            {
                throw new KeyNotFoundException("Không tìm thấy nhà cung cấp với ID này.");
            }

            return _mapper.Map<SupplierModel>(supplier);
        }

        // Cập nhật thông tin nhà cung cấp
        public async Task UpdateSupplierAsync(int id, SupplierModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Thông tin nhà cung cấp không được để trống.");
            }

            var existingSupplier = await _context.Suppliers.FindAsync(id);

            if (existingSupplier == null)
            {
                throw new KeyNotFoundException("Không tìm thấy nhà cung cấp với ID này.");
            }

            _mapper.Map(model, existingSupplier);

            _context.Suppliers.Update(existingSupplier);
            await _context.SaveChangesAsync();
        }

        // Xóa nhà cung cấp
        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                throw new KeyNotFoundException("Không tìm thấy nhà cung cấp với ID này để xóa.");
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<SupplierModel>> FindSuppliersAsync(string name, int page, int pageSize)
        {
            // Đếm tổng số nhà cung cấp có tên chứa ký tự 'name'
            var totalSuppliers = await _context.Suppliers
                .Where(s => s.SupplierName.Contains(name)) // Điều kiện tìm kiếm theo tên nhà cung cấp
                .CountAsync();

            // Lấy nhà cung cấp theo tên với phân trang
            var suppliers = await _context.Suppliers
                .Where(s => s.SupplierName.Contains(name)) // Điều kiện tìm kiếm theo tên nhà cung cấp
                .Skip((page - 1) * pageSize) // Bỏ qua các nhà cung cấp ở các trang trước
                .Take(pageSize) // Lấy số nhà cung cấp trong trang hiện tại
                .ToListAsync();

            // Ánh xạ từ Supplier sang SupplierModel
            var supplierModels = _mapper.Map<List<SupplierModel>>(suppliers);

            return new PagedResult<SupplierModel>
            {
                Items = supplierModels,
                TotalCount = totalSuppliers,
                PageSize = pageSize,
                CurrentPage = page
            };
        }
    }
}
