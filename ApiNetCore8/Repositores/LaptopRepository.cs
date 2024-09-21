using ApiNetCore8.Data;
using ApiNetCore8.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Repositores
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public LaptopRepository(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddLaptopAsync(LaptopModel model)
        {
            var newLaptop = _mapper.Map<Laptop>(model);
            _context.Laptops!.Add(newLaptop);
            await _context.SaveChangesAsync();

            return newLaptop.Id;
        }

        public async Task DeleteLaptopAsync(int id)
        {
            var deleteLaptop = _context.Laptops!.SingleOrDefault(l => l.Id == id);
            if (deleteLaptop != null)
            {
                _context.Laptops?.Remove(deleteLaptop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<LaptopModel>> GetAllLaptopsAsync()
        {
            var laptops = await _context.Laptops!.ToListAsync();
            return _mapper.Map<List<LaptopModel>>(laptops);
        }

        public async Task<Laptop> GetLaptopAsync(int id)
        {
            var laptop = await _context.Laptops!.FindAsync(id);
            return _mapper.Map<Laptop>(laptop);
        }

        public async Task UpdateLaptopAsync(int id, LaptopModel model)
        {
            if (id == model.Id)
            {
                var updateLaptop = _mapper.Map<Laptop>(model);
                _context.Laptops!.Update(updateLaptop);
                await _context.SaveChangesAsync();
            }
        }
    }
}
