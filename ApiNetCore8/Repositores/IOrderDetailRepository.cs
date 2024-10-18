using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IOrderDetailRepository
    {
        public  Task<PagedResult<OrderDetailModel>> GetAllOrderDetailAsync(int page, int pageSize);
        public Task<OrderDetailModel> GetOrderDetailByIdAsync(int id);
        public Task<int> AddOrderDetailAsync(OrderDetailModel model);
        public Task UpdateOrderDetailAsync(int id, OrderDetailModel model);
        public Task DeleteOrderDetailAsync(int id);
        public Task<PagedResult<OrderDetailModel>> FindOrderDetailAsync(string name, int page, int pageSize);

    }
}
