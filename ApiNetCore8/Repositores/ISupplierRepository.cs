using ApiNetCore8.Models;

namespace ApiNetCore8.Repositories
{
    public interface ISupplierRepository
    {
        public Task<int> AddSupplierAsync(SupplierData model);
        public Task<List<SupplierData>> GetAllSuppliersAsync();
        public Task<SupplierData> GetSupplierByIdAsync(int id);
        public Task UpdateSupplierAsync(int id, SupplierData model);
        public Task DeleteSupplierAsync(int id);
    }
}
