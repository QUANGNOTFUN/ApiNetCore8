using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IOrderDetailRepository
    {
        public Task<List<OrderDetailModel>> GetAllOrderDetailAsync(int page, int pageSize);
        public Task<OrderDetailModel> GetOrderDetailByIdAsync(int id);
        public Task<int> AddOrderDetailAsync(OrderDetailModel model);
        public Task UpdateOrderDetailAsync(int id, OrderDetailModel model);
        public Task DeleteOrderDetailAsync(int id);
        public Task FindOrderDetailsAsync(int id, int page, int pageSize);

    }
}
