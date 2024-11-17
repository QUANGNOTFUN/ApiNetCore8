using ApiNetCore8.Models;

namespace ApiNetCore8.Repositories
{
    public interface ISupplierRepository
    {
        public Task<PagedResult<SupplierModel>> GetAllSuppliersAsync(int page,int pageSize);

        public Task<PagedResult<SupplierModel>> FindSuppliersAsync(string name, int page, int pageSize);

        public Task<SupplierModel> GetSupplierByIdAsync(int id);

        public Task<int> AddSupplierAsync(SupplierModel model);

        public Task UpdateSupplierAsync(int id, SupplierModel model);

        public Task DeleteSupplierAsync(int id);
    }
}
