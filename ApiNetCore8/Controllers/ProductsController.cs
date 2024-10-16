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

        // Lấy tất cả sản phẩm với phân trang
        [HttpGet("all-product")]
        public async Task<ActionResult<PagedResult<ProductModel>>> GetAllProducts(int page, int pageSize = 20)
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


        // Lấy tất cả sản phẩm đang thiếu
        [HttpGet("low-stock")] // Đặt route là /api/products/low-stock
        // [Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetLowStockProducts(int page, int pageSize = 20)
        {
            try
            {
                var products = await _repo.GetLowStockProductsAsync(page, pageSize = 20);
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


        // Lấy sản phẩm theo ID
        [HttpGet("product{id}")]
        [Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            try
            {
                var product = await _repo.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound("Không tìm thấy sản phẩm");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // Thêm sản phẩm mới
        [HttpPost("add-product")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<ProductModel>> AddProduct(ProductModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Thông tin sản phẩm bị trống");
            }

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
                return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ProductID }, newProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message); // Phản hồi lỗi server
            }
        }

        // Cập nhật sản phẩm
        [HttpPut("update-product{id}")]
        [Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult> UpdateProduct(int id, ProductModel model)
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
                await _repo.UpdateProductAsync(id, model); // Sửa lại để sử dụng model mới
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // Xóa sản phẩm
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