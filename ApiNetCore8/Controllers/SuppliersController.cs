using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;
using Microsoft.AspNetCore.Authorization;
using ApiNetCore8.Helpers;

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

        // GET: api/Suppliers
        [HttpGet]
        [Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<IEnumerable<SupplierModel>>> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _repo.GetAllSuppliersAsync();
                if (suppliers == null || !suppliers.Any())
                {
                    return NotFound("Không có nhà cung cấp nào."); // Thêm thông báo khi không có nhà cung cấp
                }
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message); // Cung cấp thông tin chi tiết về lỗi
            }
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        [Authorize(Roles = InventoryRole.Staff)]
        public async Task<ActionResult<SupplierModel>> GetSupplierById(int id)
        {
            try
            {
                var supplier = await _repo.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    return NotFound("Không tìm thấy nhà cung cấp."); // Thêm thông báo không tìm thấy
                }
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // POST: api/Suppliers
        [HttpPost]
        [Authorize(Roles = InventoryRole.Manager)]
        public async Task<ActionResult<SupplierModel>> AddSupplier(SupplierModel model)
        {
            if (model == null)
            {
                return BadRequest("Thông tin nhà cung cấp bị trống."); // Thay đổi thông báo cho rõ ràng hơn
            }

            try
            {
                var newSupplierId = await _repo.AddSupplierAsync(model);

                if (newSupplierId <= 0)
                {
                    return BadRequest("Tạo nhà cung cấp không thành công."); // Thay đổi thông báo cho rõ ràng hơn
                }

                var newSupplier = await _repo.GetSupplierByIdAsync(newSupplierId);
                return CreatedAtAction(nameof(GetSupplierById), new { id = newSupplierId }, newSupplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        [Authorize(Roles = InventoryRole.Manager)]
        public async Task<ActionResult> UpdateSupplier(int id, SupplierModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Dữ liệu nhà cung cấp bị trống."); // Thay đổi thông báo cho rõ ràng hơn
            }

            try
            {
                var existingSupplier = await _repo.GetSupplierByIdAsync(id);
                if (existingSupplier == null)
                {
                    return NotFound("Không tìm thấy nhà cung cấp để cập nhật."); // Thêm thông báo khi không tìm thấy nhà cung cấp
                }

                await _repo.UpdateSupplierAsync(id, model);
                return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = InventoryRole.Admin)]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            try
            {
                var existingSupplier = await _repo.GetSupplierByIdAsync(id);
                if (existingSupplier == null)
                {
                    return NotFound("Không tìm thấy nhà cung cấp để xóa."); // Thêm thông báo khi không tìm thấy nhà cung cấp
                }

                await _repo.DeleteSupplierAsync(id);
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}