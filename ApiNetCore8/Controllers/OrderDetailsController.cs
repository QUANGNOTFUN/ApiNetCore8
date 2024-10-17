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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _repo;

        public OrderDetailsController(IOrderDetailRepository repo)
        {
            _repo = repo;
        }

        // GET: api/OrderDetails
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDetailModel>>> GetAllOrderDetails()
        {
            try
            {
                var orderDetails = await _repo.GetAllOrderDetailAsync();
                if (orderDetails == null || !orderDetails.Any())
                {
                    return NotFound("Không có chi tiết đơn hàng nào."); // Thêm thông báo khi không tìm thấy chi tiết
                }
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDetailModel>> GetOrderDetailById(int id)
        {
            try
            {
                var orderDetail = await _repo.GetOrderDetailByIdAsync(id);
                if (orderDetail == null)
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng."); // Thêm thông báo khi không tìm thấy
                }
                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // POST: api/OrderDetails
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDetailModel>> AddOrderDetail(OrderDetailModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu chi tiết đơn hàng bị trống."); // Thay đổi thông báo cho rõ ràng hơn
            }

            try
            {
                var newOrderDetailId = await _repo.AddOrderDetailAsync(model);

                if (newOrderDetailId <= 0)
                {
                    return BadRequest("Tạo chi tiết đơn hàng không thành công."); // Thay đổi thông báo cho rõ ràng hơn
                }

                var newOrderDetail = await _repo.GetOrderDetailByIdAsync(newOrderDetailId);

                return CreatedAtAction(nameof(GetOrderDetailById), new { id = newOrderDetailId }, newOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
        // GET: api/OrderDetails/search
        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDetailModel>>> SearchOrderDetails(string searchTerm, int page = 1, int pageSize = 10)
        {
            try
            {
                var orderDetails = await _repo.SearchOrderDetailsAsync(searchTerm, page, pageSize);

                if (orderDetails == null || !orderDetails.Any())
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng nào.");
                }

                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/OrderDetails/limited
        [HttpGet("limited")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDetailModel>>> GetLimitedOrderDetails(int limit = 20)
        {
            try
            {
                var orderDetails = await _repo.GetLimitedOrderDetailsAsync(limit);

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
    }
}
