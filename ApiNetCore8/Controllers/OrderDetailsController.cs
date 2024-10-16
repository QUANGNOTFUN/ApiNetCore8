using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                var OrderDetails = await _repo.GetAllOrderDetailAsync();
                return Ok(OrderDetails);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDetailModel>> GetOrderDetailById(int id)
        {
            try
            {
                var OrderDetail = await _repo.GetOrderDetailByIdAsync(id);
                if (OrderDetail == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy danh mục
                }
                return Ok(OrderDetail);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/OrderDetails
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDetailModel>> AddOrderDetail(OrderDetailModel model)
        {
            if (model == null)
            {
                return BadRequest("OrderDetail data is null.");
            }

            try
            {
                var newOrderDetailId = await _repo.AddOrderDetailAsync(model);

                if (newOrderDetailId <= 0)
                {
                    return BadRequest("OrderDetail creation was not successful.");
                }

                var newOrderDetail = await _repo.GetOrderDetailByIdAsync(newOrderDetailId);

                return CreatedAtAction(nameof(GetOrderDetailById), new { id = newOrderDetailId }, newOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // PUT: api/OrderDetails/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateOrderDetail(int id, OrderDetailModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("OrderDetail data is null.");
            }

            try
            {
                // Kiểm tra xem danh mục có tồn tại không
                var existingOrderDetail = await _repo.GetOrderDetailByIdAsync(id);
                if (existingOrderDetail == null)
                {
                    return NotFound();
                }

                await _repo.UpdateOrderDetailAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                var existingOrderDetail = await _repo.GetOrderDetailByIdAsync(id);
                if (existingOrderDetail == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy danh mục
                }

                await _repo.DeleteOrderDetailAsync(id); // Gọi phương thức xóa
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
