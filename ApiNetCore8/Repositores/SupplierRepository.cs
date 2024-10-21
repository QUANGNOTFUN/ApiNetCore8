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
            model.SupplierId = 0; // Đảm bảo ID là auto-increment

            var newSupplier = _mapper.Map<Supplier>(model); // Mapping từ SupplierModel sang Supplier entity

            await _context.Suppliers.AddAsync(newSupplier);
            await _context.SaveChangesAsync();

            return newSupplier.SupplierId;
        }

        // Lấy tất cả nhà cung cấp
        public async Task<List<SupplierModel>> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return _mapper.Map<List<SupplierModel>>(suppliers);
        }

        // Lấy nhà cung cấp theo ID
        public async Task<SupplierModel> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.Suppliers.SingleOrDefaultAsync(c => c.SupplierId == id);

            if (supplier == null)
            {
                throw new KeyNotFoundException("Không có nhà cung cấp tồn tại");
            }

            return _mapper.Map<SupplierModel>(supplier);
        }

        // Cập nhật thông tin nhà cung cấp
        public async Task UpdateSupplierAsync(int id, SupplierModel model)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(id);

            if (existingSupplier == null)
            {
                throw new KeyNotFoundException("Không có nhà cung cấp tồn tại");
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
                throw new KeyNotFoundException("Không có nhà cung cấp tồn tại");
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public Task<List<SupplierModel>> GetAllSuppliersAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task FindSupplierAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
