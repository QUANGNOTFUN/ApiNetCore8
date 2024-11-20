using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using Microsoft.AspNetCore.Authorization;
using ApiNetCore8.Repositores;
using System.Drawing.Printing;
using ApiNetCore8.Data;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository repo, IProductRepository productRepository)
        {
            _repo = repo;
            _productRepository = productRepository;

        }

        // GET: api/Orders
        [HttpGet("all-Orders")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAllOrders(int page = 1, int pageSize = 20)
        {
            try
            {
                var pagedOrder = await _repo.GetAllOrderAsync(page, pageSize);

                if (pagedOrder == null || !pagedOrder.Items.Any())
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng.");
                }

                return Ok(pagedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/Orders/5
        [HttpGet("find-OrdersByDate")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrdersByDate([FromQuery] DateTime date)
        {
            try
            {
                var orders = await _repo.GetOrdersByDateAsync(date);
                if (orders == null || !orders.Any())
                {
                    return NotFound("Không tìm thấy đơn hàng vào ngày này.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }



        // POST: api/Orders
        // API Thêm đơn hàng
        [HttpPost("add-order")]
        [Authorize]
        public async Task<IActionResult> AddOrder(string button, [FromBody] addOrderModel model)
        {
            if (model == null || !model.addOrderDetails.Any())
                return BadRequest("Dữ liệu đơn hàng không hợp lệ.");

            try
            {
                var newOrderId = await _repo.AddOrderAsync(button, model);
                return Ok(new { Message = "Đơn hàng đã được thêm.", OrderId = newOrderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }


        [HttpPut("update-order-status")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string action)
        {
            try
            {
                string status;

                if (action == "confirm")
                {
                    status = "Successful";
                }
                else if (action == "cancel")
                {
                    status = "Cancel";
                }
                else
                {
                    return BadRequest("Hành động không hợp lệ.");
                }

               
                await _repo.UpdateOrderStatusAndQuantityAsync(orderId, status, action);

                return Ok($"Trạng thái và số lượng của đơn hàng đã được cập nhật: {status}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

    }
}
