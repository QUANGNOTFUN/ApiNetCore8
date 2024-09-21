using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILaptopRepository _laptopRepo;

        public ProductsController(ILaptopRepository repo) 
        {
            _laptopRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLaptops()
        {
            try
            {
                return Ok(await _laptopRepo.GetAllLaptopsAsync());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLaptopById(int id)
        {
            var laptop = await _laptopRepo.GetLaptopAsync(id);
            return laptop == null ? NotFound() : Ok(laptop);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewLaptop(LaptopModel model)
        {
            try
            {
                var newLaptopId = await _laptopRepo.AddLaptopAsync(model);
                var laptop = await _laptopRepo.GetLaptopAsync(newLaptopId);
                return laptop == null ? NotFound() : Ok(laptop);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
