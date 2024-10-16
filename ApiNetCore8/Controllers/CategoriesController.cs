using System;
using System.Collections.Generic;
using System.Linq; // Thêm vào để sử dụng phương thức Any()
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories; // Đảm bảo đây là đường dẫn đúng
using Microsoft.AspNetCore.Authorization;
using ApiNetCore8.Helpers;
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
        [HttpGet("all-category")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategories(int page = 1, int pageSize = 20)
        {
            try
            {
                var categories = await _repo.GetAllCategoryAsync(page, pageSize);

                if (categories == null || !categories.Items.Any())
                {
                    return NotFound("Không có danh mục sản phẩm nào.");
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/Categories/find-category?name={name}
        [HttpGet("find-category")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<List<CategoryModel>>> GetCategoryByName(string name, int page = 1, int pageSize = 20)
        {
            try
            {
                // Tìm danh mục có tên chứa chuỗi ký tự 'name' (không phân biệt hoa thường)
                var categories = await _repo.GetCategoriesByNameAsync(name, page, pageSize);

                if (categories == null || !categories.Items.Any())
                {
                    return NotFound("Không tìm thấy danh mục."); // Thông báo nếu không có kết quả
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message); // Xử lý lỗi hệ thống
            }
        }


        // POST: api/Categories
        [HttpPost("add-category")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<CategoryModel>> AddCategory(CategoryModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu danh mục bị trống.");
            }

            if (!ModelState.IsValid) // Kiểm tra tính hợp lệ của model
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newCategoryId = await _repo.AddCategoryAsync(model);
                if (newCategoryId <= 0)
                {
                    return BadRequest("Tạo danh mục không thành công.");
                }

                var newCategory = await _repo.GetCategoryByIdAsync(newCategoryId);
                return CreatedAtAction(nameof(AddCategory), new { id = newCategoryId }, newCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // PUT: api/Categories/5
        [HttpPut("update-category")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult> UpdateCategory(int id, CategoryModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Dữ liệu danh mục bị trống.");
            }

            try
            {
                // Kiểm tra xem danh mục có tồn tại không
                var existingCategory = await _repo.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound("Không tìm thấy danh mục."); // Thêm thông báo chi tiết
                }

                await _repo.UpdateCategoryAsync(id, model);
                return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("delete-category")]
        //[Authorize(Roles = InventoryRole.Admin)]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var existingCategory = await _repo.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound("Không tìm thấy danh mục."); // Thêm thông báo chi tiết
                }

                await _repo.DeleteCategoryAsync(id); // Gọi phương thức xóa
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
