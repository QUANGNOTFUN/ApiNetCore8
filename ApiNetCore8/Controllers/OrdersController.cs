using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/Orders/all-Order?page={page}&pageSize={pageSize}
        [HttpGet("all-Order")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAllOrders(int page = 1, int pageSize = 20)
        {
            try
            {
                var orders = await _repo.GetAllOrderAsync(page, pageSize);

                if (orders == null || !orders.Items.Any())
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

        // GET: api/Orders/find-Order?id={id}&page={page}&pageSize={pageSize}
        [HttpGet("find-Order")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<IEnumerable<OrderModel>>> FindOrder(int id, int page = 1, int pageSize = 20)
        {
            try
            {
                var orders = await _repo.FindOrdersAsync(id, page, pageSize);

                if (orders == null || !orders.Items.Any())
                {
                    return NotFound("Không tìm thấy đơn hàng.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // POST: api/Orders/add-Order
        [HttpPost("add-Order")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<OrderModel>> AddOrder(OrderModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu đơn hàng bị trống.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newOrderId = await _repo.AddOrderAsync(model);
                if (newOrderId <= 0)
                {
                    return BadRequest("Tạo đơn hàng không thành công.");
                }

                var newOrder = await _repo.GetOrderByIdAsync(newOrderId);
                return CreatedAtAction(nameof(AddOrder), new { id = newOrderId }, newOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // PUT: api/Orders/update-Order?id={id}
        [HttpPut("update-Order")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult> UpdateOrder(int id, OrderModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu đơn hàng bị trống.");
            }

            try
            {
                var existingOrder = await _repo.GetOrderByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound("Không tìm thấy đơn hàng.");
                }

                await _repo.UpdateOrderAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // DELETE: api/Orders/delete-Order?id={id}
        [HttpDelete("delete-Order")]
        //[Authorize(Roles = InventoryRole.Admin)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var existingOrder = await _repo.GetOrderByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound("Không tìm thấy đơn hàng.");
                }

                await _repo.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
