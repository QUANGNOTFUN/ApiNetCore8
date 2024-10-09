using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAllCategories()
        {
            try
            {
                var categories = await _repo.GetAllOrderAsync();
                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrderById(int id)
        {
            try
            {
                var Order = await _repo.GetOrderByIdAsync(id);
                if (Order == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy danh mục
                }
                return Ok(Order);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<OrderModel>> AddOrder(OrderModel model)
        {
            if (model == null)
            {
                return BadRequest("Order data is null.");
            }

            try
            {
                var newOrderId = await _repo.AddOrderAsync(model);

                if (newOrderId <= 0)
                {
                    return BadRequest("Order creation was not successful.");
                }

                var newOrder = await _repo.GetOrderByIdAsync(newOrderId);

                return CreatedAtAction(nameof(GetOrderById), new { id = newOrderId }, newOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, OrderModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Order data is null.");
            }

            try
            {
                // Kiểm tra xem danh mục có tồn tại không
                var existingOrder = await _repo.GetOrderByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }

                await _repo.UpdateOrderAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var existingOrder = await _repo.GetOrderByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy danh mục
                }

                await _repo.DeleteOrderAsync(id); // Gọi phương thức xóa
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
