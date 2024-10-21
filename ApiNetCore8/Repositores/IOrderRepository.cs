using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IOrderRepository
    {
        public Task<OrderModel> CheckNullModelAsync(int id, OrderModel model);

        public Task<PagedResult<OrderModel>> GetAllOrderAsync(int page, int pageSize);

        public Task<PagedResult<OrderModel>> FindOrdersAsync(int id, int page, int pageSize);

        public Task<OrderModel> GetOrderByIdAsync(int id);

        public Task<int> AddOrderAsync(OrderModel model);

        public Task UpdateOrderAsync(int id, OrderModel model);

        public Task DeleteOrderAsync(int id);
        Task FindSuppliersAsync(int id, int page, int pageSize);
    }
}
