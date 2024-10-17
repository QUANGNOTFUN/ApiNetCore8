using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IOrderDetailRepository
    {
        public Task<List<OrderDetailModel>> GetAllOrderDetailAsync();
        public Task<OrderDetailModel> GetOrderDetailByIdAsync(int id);
        public Task<int> AddOrderDetailAsync(OrderDetailModel model);
        public Task UpdateOrderDetailAsync(int id, OrderDetailModel model);
        public Task DeleteOrderDetailAsync(int id);
        Task<List<OrderDetailModel>> SearchOrderDetailsAsync(string searchTerm, int page, int pageSize);
        Task<List<OrderDetailModel>> GetLimitedOrderDetailsAsync(int limit);
    }
}
