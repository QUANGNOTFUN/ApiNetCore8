using ApiNetCore8.Models;

namespace ApiNetCore8.Repositories
{
    public interface IProductRepository
    {
        public Task<PagedResult<ProductModel>> GetAllProductsAsync(int page, int pageSize);

        public Task<PagedResult<LowProductModel>> GetLowStockProductsAsync(int page, int pageSize);

        public Task<PagedResult<ProductModel>> FindProductsAsync(string name, int page, int pageSize);

        public Task<ProductModel> GetProductByIdAsync(int id);

        public Task<int> AddProductAsync(InputProductModel model);

        public Task UpdateProductAsync(int id, InputProductModel model);

        public Task DeleteProductAsync(int id);
    }
}
