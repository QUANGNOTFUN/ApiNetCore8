using ApiNetCore8.Data;
using ApiNetCore8.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

        // Lấy tất cả nhà cung cấp
        public async Task<List<SupplierModel>> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers.ToListAsync();

            if (!suppliers.Any())
            {
                throw new KeyNotFoundException("Không có nhà cung cấp nào tồn tại.");
            }

            return _mapper.Map<List<SupplierModel>>(suppliers);
        }

        // Lấy tất cả nhà cung cấp với phân trang
        public async Task<List<SupplierModel>> GetAllSuppliersAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Số trang và kích thước trang phải lớn hơn 0.");
            }

            var suppliers = await _context.Suppliers
                .Include(s => s.Categories)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!suppliers.Any())
            {
                throw new KeyNotFoundException("Không có nhà cung cấp nào tồn tại trên trang này.");
            }

            return _mapper.Map<List<SupplierModel>>(suppliers);
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

        public Task FindSupplierAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
