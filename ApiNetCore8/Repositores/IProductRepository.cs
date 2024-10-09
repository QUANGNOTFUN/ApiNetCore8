using ApiNetCore8.Models;

namespace ApiNetCore8.Repositories
{
    public interface IProductRepository
    {
        public Task<List<ProductModel>> GetAllProductsAsync();
        public Task<ProductModel> GetProductByIdAsync(int id);
        public Task<int> AddProductAsync(ProductModel model);
        public Task UpdateProductAsync(int id, ProductModel model);
        public Task DeleteProductAsync(int id);
    }
}
