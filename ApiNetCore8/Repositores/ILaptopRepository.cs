using ApiNetCore8.Data;
using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface ILaptopRepository
    {
        public Task<List<LaptopModel>> GetAllLaptopsAsync();
        public Task<Laptop> GetLaptopAsync(int id);
        public Task<int> AddLaptopAsync(LaptopModel model);
        public Task UpdateLaptopAsync(int id, LaptopModel model);
        public Task DeleteLaptopAsync(int id);
    }
}
