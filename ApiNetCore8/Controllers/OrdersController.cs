﻿using System;
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
        //[Authorize]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAllOrders(int page, int pageSize)
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
        [HttpGet("find-Order")]
        //[Authorize]
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


        // POST: api/Orders
        // API Thêm đơn hàng
        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder(string button, OrderModel model, string action)
        {
            if (model == null)
                return BadRequest("Dữ liệu đơn hàng bị trống.");

            try
            {
                if (action == "cancel")
                {
                    model.Status = "Cancel";
                    var newOrderId = await _repo.AddOrderAsync(button, model);
                    await _repo.UpdateOrderStatusAsync(newOrderId, "Cancel");
                    return Ok("Đơn hàng đã hủy.");
                }

                if (action == "confirm")
                {
                    model.Status = "Successful";
                    var newOrderId = await _repo.AddOrderAsync(button, model);
                    await _repo.UpdateOrderStatusAsync(newOrderId, "Successful");
                    return Ok("Đơn hàng đã xác nhận.");
                }

                return BadRequest("Hành động không hợp lệ.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        [HttpPut("update-Order")]
        //[Authorize]
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
        [HttpDelete("delete-Order")]
        //[Authorize]
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
