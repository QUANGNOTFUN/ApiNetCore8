using ApiNetCore8.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using Microsoft.AspNetCore.Authorization;
using ApiNetCore8.Helpers;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Products/all-product
        [HttpGet("all-product")]
        public async Task<ActionResult<PagedResult<ProductModel>>> GetAllProducts(int page = 1, int pageSize = 20)
        {
            try
            {
                var result = await _repo.GetAllProductsAsync(page, pageSize);
                if (result.Items == null || !result.Items.Any())
                {
                    return NotFound("Không có sản phẩm!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }


        // GET: api/Products/low-stock?page={page}&pageSize={pageSize}
        [HttpGet("low-stock")]
        // [Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<IEnumerable<LowProductModel>>> GetLowStockProducts(int page = 1, int pageSize = 20)
        {
            try
            {
                var products = await _repo.GetLowStockProductsAsync(page, pageSize);
                if (products == null || !products.Items.Any())
                {
                    return NotFound("Không có sản phẩm đang thiếu!");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/products/find-product?name={name}
        [HttpGet("find-product")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<List<ProductModel>>> FindProduct(string name, int page = 1, int pageSize = 20)
        {
            try
            {
                // Tìm danh mục có tên chứa chuỗi ký tự 'name' (không phân biệt hoa thường)
                var products = await _repo.FindProductsAsync(name, page, pageSize);

                if (products == null || !products.Items.Any())
                {
                    return NotFound("Không tìm thấy danh mục."); // Thông báo nếu không có kết quả
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message); // Xử lý lỗi hệ thống
            }
        }

        // Lấy sản phẩm theo ID
        //[HttpGet("product{id}")]
        //[Authorize(Roles = InventoryRole.Staff)]
        //public async Task<ActionResult<ProductModel>> GetProductById(int id)
        //{
        //    try
        //    {
        //        var product = await _repo.GetProductByIdAsync(id);
        //        if (product == null)
        //        {
        //            return NotFound("Không tìm thấy sản phẩm");
        //        }
        //        return Ok(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
        //    }
        //}

        // POST: api/Products/add-product?model={formbody}
        [HttpPost("add-product")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<InputProductModel>> AddProduct(InputProductModel model)
        {
            // Xác thực model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Phản hồi lỗi với các thông báo xác thực
            }

            try
            {
                var newProductId = await _repo.AddProductAsync(model);
                var newProduct = await _repo.GetProductByIdAsync(newProductId);

                // Trả về kết quả thành công
                return CreatedAtAction(nameof(AddProduct), new { id = newProduct.ProductID }, newProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message); // Phản hồi lỗi server
            }
        }

        // PUT: api/Products/update-product?id={id}&model={formbody}
        [HttpPut("update-product")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult> UpdateProduct(int id, InputProductModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu sản phẩm bị trống.");
            }

            try
            {
                var existingProduct = await _repo.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy sản phẩm để cập nhật.");
                }

                // Cập nhật thông tin sản phẩm
                await _repo.UpdateProductAsync(id, model); 
                return Ok("Đã cập nhật thành công sản phẩm");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // DELETE: api/Products/deleete-product?id={id}
        //[HttpDelete("delete-product{id}")]
        //[Authorize(Roles = InventoryRole.Admin)]
        //public async Task<ActionResult> DeleteProduct(int id)
        //{
        //    try
        //    {
        //        var existingProduct = await _repo.GetProductByIdAsync(id);
        //        if (existingProduct == null)
        //        {
        //            return NotFound("Không tìm thấy sản phẩm để xóa.");
        //        }

        //        await _repo.DeleteProductAsync(id);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
        //    }
        //}
    }
}