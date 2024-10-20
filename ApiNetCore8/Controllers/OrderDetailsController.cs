using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using Microsoft.AspNetCore.Authorization;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _repo;

        public OrderDetailsController(IOrderDetailRepository repo)
        {
            _repo = repo;
        }

        // GET: api/OrderDetails/all-OrderDetails?page={page}&pageSize={pageSize}
        [HttpGet("all-OrderDetails")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<IEnumerable<OrderDetailModel>>> GetAllOrderDetails(int page = 1, int pageSize = 20)
        {
            try
            {
                var orderDetails = await _repo.GetAllOrderDetailAsync(page, pageSize);

                if (orderDetails == null || !orderDetails.Any())
                {
                    return NotFound("Không có chi tiết đơn hàng nào.");
                }

                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/OrderDetails/find-OrderDetail?id={id}&page={page}&pageSize={pageSize}
        [HttpGet("find-OrderDetail")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<OrderDetailModel>> FindOrderDetail(int id)
        {
            try
            {
                var orderDetail = await _repo.GetOrderDetailByIdAsync(id);

                if (orderDetail == null)
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng.");
                }

                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // POST: api/OrderDetails/add-OrderDetail
        [HttpPost("add-OrderDetail")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<OrderDetailModel>> AddOrderDetail(OrderDetailModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu chi tiết đơn hàng bị trống.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newOrderDetailId = await _repo.AddOrderDetailAsync(model);
                if (newOrderDetailId <= 0)
                {
                    return BadRequest("Tạo chi tiết đơn hàng không thành công.");
                }

                var newOrderDetail = await _repo.GetOrderDetailByIdAsync(newOrderDetailId);
                return CreatedAtAction(nameof(AddOrderDetail), new { id = newOrderDetailId }, newOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // PUT: api/OrderDetails/update-OrderDetail?id={id}
        [HttpPut("update-OrderDetail")]
        //[Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult> UpdateOrderDetail(int id, OrderDetailModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu chi tiết đơn hàng bị trống.");
            }

            try
            {
                var existingOrderDetail = await _repo.GetOrderDetailByIdAsync(id);
                if (existingOrderDetail == null)
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng.");
                }

                await _repo.UpdateOrderDetailAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // DELETE: api/OrderDetails/delete-OrderDetail?id={id}
        [HttpDelete("delete-OrderDetail")]
        //[Authorize(Roles = InventoryRole.Admin)]
        public async Task<ActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                var existingOrderDetail = await _repo.GetOrderDetailByIdAsync(id);
                if (existingOrderDetail == null)
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng.");
                }

                await _repo.DeleteOrderDetailAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
