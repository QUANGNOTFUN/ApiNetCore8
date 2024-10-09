using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryModel>> GetAllCategoryAsync();
        public Task<CategoryModel> GetCategoryByIdAsync(int id);
        public Task<int> AddCategoryAsync(CategoryModel model);
        public Task UpdateCategoryAsync(int id, CategoryModel model);
        public Task DeleteCategoryAsync(int id);
    }
}
