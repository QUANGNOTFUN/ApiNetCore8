using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _repo;

        public SuppliersController(ISupplierRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Suppliers/all-Supplier?page={page}&pageSize={pageSize}
        [HttpGet("all-Supplier")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SupplierModel>>> GetAllSuppliers(int page = 1, int pageSize = 20)
        {
            try
            {
                var suppliers = await _repo.GetAllSuppliersAsync(page, pageSize);

                if (suppliers == null || !suppliers.Items.Any())
                {
                    return NotFound("Không có nhà cung cấp nào.");
                }

                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // GET: api/Suppliers/find-Supplier?id={id}
        [HttpGet("find-Supplier")]
        [Authorize]
        public async Task<ActionResult<SupplierModel>> FindSupplier(string name, int page = 1, int pageSize = 20)
        {
            try
            {
                var suppliers = await _repo.FindSuppliersAsync(name, page, pageSize);

                if (suppliers == null)
                {
                    return NotFound("Không tìm thấy nhà cung cấp.");
                }

                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // POST: api/Suppliers/add-Supplier
        [HttpPost("add-Supplier")]
        [Authorize]
        public async Task<ActionResult<SupplierModel>> AddSupplier(SupplierModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu nhà cung cấp bị trống.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newSupplierId = await _repo.AddSupplierAsync(model);
                if (newSupplierId <= 0)
                {
                    return BadRequest("Tạo nhà cung cấp không thành công.");
                }

                var newSupplier = await _repo.GetSupplierByIdAsync(newSupplierId);
                return CreatedAtAction(nameof(AddSupplier), new { id = newSupplierId }, newSupplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // PUT: api/Suppliers/update-Supplier?id={id}
        [HttpPut("update-Supplier")]
        [Authorize]
        public async Task<ActionResult> UpdateSupplier(int id, SupplierModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu nhà cung cấp bị trống.");
            }

            try
            {
                var existingSupplier = await _repo.GetSupplierByIdAsync(id);
                if (existingSupplier == null)
                {
                    return NotFound("Không tìm thấy nhà cung cấp.");
                }

                await _repo.UpdateSupplierAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // DELETE: api/Suppliers/delete-Supplier?id={id}
        [HttpDelete("delete-Supplier")]
        [Authorize]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            try
            {
                var existingSupplier = await _repo.GetSupplierByIdAsync(id);
                if (existingSupplier == null)
                {
                    return NotFound("Không tìm thấy nhà cung cấp.");
                }

                await _repo.DeleteSupplierAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
