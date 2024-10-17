using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        // GET: api/Orders
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAllOrders()
        {
            try
            {
                var orders = await _repo.GetAllOrderAsync();
                if (orders == null || !orders.Any())
                {
                    return NotFound("Không có đơn hàng nào.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderModel>> GetOrderById(int id)
        {
            try
            {
                var order = await _repo.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound("Không tìm thấy đơn hàng.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // POST: api/Orders
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderModel>> AddOrder(OrderModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu đơn hàng bị trống.");
            }

            try
            {
                var newOrderId = await _repo.AddOrderAsync(model);

                if (newOrderId <= 0)
                {
                    return BadRequest("Tạo đơn hàng không thành công.");
                }

                var newOrder = await _repo.GetOrderByIdAsync(newOrderId);

                return CreatedAtAction(nameof(GetOrderById), new { id = newOrderId }, newOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/Orders/search
        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderModel>>> SearchOrders(string searchTerm, int page = 1, int pageSize = 10)
        {
            try
            {
                // Gọi phương thức với 3 tham số: searchTerm, page, pageSize
                var orders = await _repo.SearchOrdersAsync(searchTerm, page, pageSize);

                if (orders == null || !orders.Any())
                {
                    return NotFound("Không tìm thấy đơn hàng nào.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
        // GET: api/Orders/limited
        [HttpGet("limited")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetLimitedOrders(int limit=20)
        {
            try
            {
                // Gọi phương thức với tham số limit
                var orders = await _repo.GetLimitedOrdersAsync(limit);

                if (orders == null || !orders.Any())
                {
                    return NotFound("Không có đơn hàng nào.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

    }
}
