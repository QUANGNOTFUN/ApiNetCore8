﻿using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IOrderRepository
    {
        public Task<PagedResult<OrderModel>> GetAllOrderAsync(int page, int pageSize);
        public Task<int> AddOrderAsync(string button, addOrderModel model);
        public Task UpdateOrderStatusAndQuantityAsync(int orderId, string status, string action);
        public Task UpdateOrderAsync(int id, OrderModel model);
        public Task DeleteOrderAsync(int id);
        public Task<OrderModel> GetOrderByIdAsync(int id);
        public Task<PagedResult<OrderModel>> FindOrderAsync(string name, int page, int pageSize);
        public Task<IEnumerable<OrderModel>> GetOrdersByDateAsync(DateTime date);

    }
}
