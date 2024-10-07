using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryData>> GetAllCategoryAsync();
        public Task<CategoryData> GetCategoryByIdAsync(int id);
        public Task<int> AddCategoryAsync(CategoryData model);
        public Task UpdateCategoryAsync(int id, CategoryData model);
        public Task DeleteCategoryAsync(int id);
    }
}
