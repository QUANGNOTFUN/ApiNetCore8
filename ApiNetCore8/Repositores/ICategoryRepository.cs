using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface ICategoryRepository
    {

        public Task<PagedResult<CategoryModel>> GetAllCategoryAsync(int page, int pageSize);

        public Task<PagedResult<CategoryModel>> FindCategoriesAsync(string name, int page, int pageSize);

        public Task<CategoryModel> GetCategoryByIdAsync(int id);

        public Task<int> AddCategoryAsync(CategoryModel model);

        public Task UpdateCategoryAsync(int id, CategoryModel model);

        public Task DeleteCategoryAsync(int id);
    }
}
