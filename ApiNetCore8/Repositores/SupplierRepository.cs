using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
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

        // Thêm nhà cung cấp mới
        public async Task<int> AddSupplierAsync(SupplierModel model)
        {
            var newSupplier = _mapper.Map<Supplier>(model);

            // Kiểm tra xem nhà cung cấp đã tồn tại chưa
            var existingSupplier = await _context.Suppliers
                                                 .FirstOrDefaultAsync(e => e.SupplierName == newSupplier.SupplierName);

            if (existingSupplier != null)
            {
                throw new ArgumentException("SupplierName already exists.");
            }

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
            var supplier = await _context.Suppliers.FindAsync(id);
            return _mapper.Map<SupplierModel>(supplier);
        }

        // Cập nhật thông tin nhà cung cấp
        public async Task UpdateSupplierAsync(int id, SupplierModel model)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }

            // Ánh xạ thông tin từ SupplierData sang Supplier
            _mapper.Map(model, supplier);

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        // Xóa nhà cung cấp theo ID
        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Supplier not found");
            }
        }
    }
}
