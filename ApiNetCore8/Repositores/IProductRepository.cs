using ApiNetCore8.Models;

namespace ApiNetCore8.Repositories
{
    public interface IProductRepository
    {
        public Task<List<ProductData>> GetAllProductsAsync();
        public Task<ProductData> GetProductByIdAsync(int id);
        public Task<int> AddProductAsync(ProductData model);
        public Task UpdateProductAsync(int id, ProductData model);
        public Task DeleteProductAsync(int id);
    }
}
