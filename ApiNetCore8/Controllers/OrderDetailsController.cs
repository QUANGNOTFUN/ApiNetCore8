using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using Microsoft.AspNetCore.Authorization;
using ApiNetCore8.Repositores;
using System.Drawing.Printing;

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
        [HttpGet("all-OrderDetails")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDetailModel>>> GetAllOrderDetails(int page = 1, int pageSize = 20)
        {
            try
            {
                var pagedOrderDetails = await _repo.GetAllOrderDetailAsync(page, pageSize);

                if (pagedOrderDetails == null || !pagedOrderDetails.Items.Any())
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng.");
                }

                return Ok(pagedOrderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/OrderDetails/5
        [HttpGet("Get-OrderDetail")]
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
    }
}

