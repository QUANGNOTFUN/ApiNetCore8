using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IOrderRepository
    {
        public Task<PagedResult<OrderModel>> GetAllOrderAsync(int page, int pageSize);
        public Task<int> AddOrderAsync(string button,OrderModel model);
        public Task UpdateOrderStatusAsync(int orderId, string status);
        public Task UpdateOrderAsync(int id, OrderModel model);
        public Task DeleteOrderAsync(int id);
        public Task<OrderModel> GetOrderByIdAsync(int id);
        public Task<PagedResult<OrderModel>> FindOrderAsync(string name, int page, int pageSize);
    }
}
