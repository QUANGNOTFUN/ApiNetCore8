using ApiNetCore8.Models;

namespace ApiNetCore8.Repositories
{
    public interface IProductRepository
    {
        public Task<PagedResult<ProductModel>> GetAllProductsAsync(int page, int pageSize);

        public Task<PagedResult<ProductModel>> GetLowStockProductsAsync(int page, int pageSize);

        public Task<PagedResult<ProductModel>> FindProductsAsync(string name, int page, int pageSize);

        public Task<ProductModel> GetProductByIdAsync(int id);

        public Task<ProductModel> CheckNullProductModel(int id, UpdateProductModel model);

        public Task<int> AddProductAsync(ProductModel model);

        public Task UpdateProductAsync(int id, UpdateProductModel model);

        public Task DeleteProductAsync(int id);
    }
}
