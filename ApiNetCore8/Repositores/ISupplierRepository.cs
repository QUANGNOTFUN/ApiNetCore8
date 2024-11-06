using ApiNetCore8.Models;

namespace ApiNetCore8.Repositories
{
    public interface ISupplierRepository
    {
        public Task<int> AddSupplierAsync(SupplierModel model);
        public Task<List<SupplierModel>> GetAllSuppliersAsync();
        public Task<SupplierModel> GetSupplierByIdAsync(int id);
        public Task UpdateSupplierAsync(int id, SupplierModel model);
        public Task DeleteSupplierAsync(int id);
    }
}
