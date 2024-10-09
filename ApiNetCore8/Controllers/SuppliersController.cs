using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore8.Models;
using ApiNetCore8.Repositories;

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
        public async Task<ActionResult<IEnumerable<SupplierModel>>> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _repo.GetAllSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierModel>> GetSupplierById(int id)
        {
            try
            {
                var supplier = await _repo.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy nhà cung cấp
                }
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<ActionResult<SupplierModel>> AddSupplier(SupplierModel model)
        {
            if (model == null)
            {
                return BadRequest("Supplier data is null.");
            }

            try
            {
                var newSupplierId = await _repo.AddSupplierAsync(model);

                if (newSupplierId <= 0)
                {
                    return BadRequest("Supplier creation was not successful.");
                }

                var newSupplier = await _repo.GetSupplierByIdAsync(newSupplierId);

                return CreatedAtAction(nameof(GetSupplierById), new { id = newSupplierId }, newSupplier);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSupplier(int id, SupplierModel model)
        {
            if (model == null) // Kiểm tra nếu model là null
            {
                return BadRequest("Supplier data is null.");
            }

            try
            {
                // Kiểm tra xem nhà cung cấp có tồn tại không
                var existingSupplier = await _repo.GetSupplierByIdAsync(id);
                if (existingSupplier == null)
                {
                    return NotFound();
                }

                await _repo.UpdateSupplierAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            try
            {
                var existingSupplier = await _repo.GetSupplierByIdAsync(id);
                if (existingSupplier == null)
                {
                    return NotFound(); // Trả về 404 nếu không tìm thấy nhà cung cấp
                }

                await _repo.DeleteSupplierAsync(id); // Gọi phương thức xóa
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
