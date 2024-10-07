using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddCategoryAsync(CategoryData model)
        {
            var newCategory = _mapper.Map<Category>(model);

            var existingCategory = await _context.Categories
                                                 .FirstOrDefaultAsync(e => e.CategoryName == newCategory.CategoryName);

            if (existingCategory != null)
            {
                throw new ArgumentException("CategoryName already exists.");
            }

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return newCategory.CategoryId;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Category not found");
            }
        }

        public async Task<List<CategoryData>> GetAllCategoryAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryData>>(categories);
        }

        public async Task<CategoryData> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return _mapper.Map<CategoryData>(category);
        }

        public async Task UpdateCategoryAsync(int id, CategoryData model)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            _mapper.Map(model, category);

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
