using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IOrderRepository
    {
        public Task<List<OrderModel>> GetAllOrderAsync();
        public Task<OrderModel> GetOrderByIdAsync(int id);
        public Task<int> AddOrderAsync(OrderModel model);
        public Task UpdateOrderAsync(int id, OrderModel model);
        public Task DeleteOrderAsync(int id);
        public Task<List<OrderModel>> GetLimitedOrdersAsync(int limit);
        public Task<List<OrderModel>> SearchOrdersAsync(string searchTerm, int page, int pageSize);
    }
}
