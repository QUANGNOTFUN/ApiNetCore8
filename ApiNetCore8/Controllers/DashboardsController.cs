using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashBoardRepository _repo;

        public DashboardsController(IDashBoardRepository repo) 
        {
            _repo = repo;
        }

        [HttpGet("get-dashboard-in-day")]
        public async Task<ActionResult<IEnumerable<DashBoardModel>>> GetDashBoardInDay()
        {
            try
            {
                var dashBoard = await _repo.GetDashBoardInDayAsync();

                if (dashBoard == null)
                {
                    return NotFound("Không có DashBoard được tạo");
                }

                return Ok(dashBoard);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        [HttpGet("get-dashboard-all")]
        public async Task<ActionResult<IEnumerable<DashBoardModel>>> GetDashBoardAll()
        {
            try
            {
                var dashBoard = await _repo.GetDashBoardAllAsync();

                if (dashBoard == null)
                {
                    return NotFound("Không có DashBoard được tạo");
                }

                return Ok(dashBoard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var summary = await _repo.GetSummaryAsync();

                if (summary == null)
                {
                    return NotFound("Không có dữ liệu tổng hợp.");
                }

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}
