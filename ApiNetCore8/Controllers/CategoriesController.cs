using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using ApiNetCore8.Repositores;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;

        public CategoriesController(ICategoryRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryData>>> GetAllCategories()
        {
            try
            {
                var categories = await _repo.GetAllCategoryAsync();
                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryData>> GetCategoryById(int id)
        {
            try
            {
                var category = await _repo.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy danh mục
                }
                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryData>> AddCategory(CategoryData model)
        {
            if (model == null)
            {
                return BadRequest("Category data is null.");
            }

            try
            {
                var newCategoryId = await _repo.AddCategoryAsync(model);

                if (newCategoryId <= 0)
                {
                    return BadRequest("Category creation was not successful.");
                }

                var newCategory = await _repo.GetCategoryByIdAsync(newCategoryId);

                return CreatedAtAction(nameof(GetCategoryById), new { id = newCategoryId }, newCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryData model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Category data is null.");
            }

            try
            {
                // Kiểm tra xem danh mục có tồn tại không
                var existingCategory = await _repo.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound();
                }

                await _repo.UpdateCategoryAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var existingCategory = await _repo.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy danh mục
                }

                await _repo.DeleteCategoryAsync(id); // Gọi phương thức xóa
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
