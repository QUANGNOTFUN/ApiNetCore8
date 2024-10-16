using ApiNetCore8.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using Microsoft.AspNetCore.Authorization;

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

        // Lấy tất cả sản phẩm
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts()
        {
            try
            {
                var products = await _repo.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            try
            {
                var product = await _repo.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy sản phẩm
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Thêm sản phẩm mới
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductModel>> AddProduct(ProductModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Product data is null.");
            }

            try
            {
                var newProductId = await _repo.AddProductAsync(model);
                var newProduct = await _repo.GetProductByIdAsync(newProductId);
                return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ProductID }, newProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Cập nhật sản phẩm
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateProduct(int id, ProductModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Product data is null.");
            }

            try
            {
                var existingProduct = await _repo.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy sản phẩm
                }

                await _repo.UpdateProductAsync(id, existingProduct); // Gọi phương thức cập nhật
                return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Xóa sản phẩm
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await _repo.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy sản phẩm
                }

                await _repo.DeleteProductAsync(id); // Gọi phương thức xóa
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
