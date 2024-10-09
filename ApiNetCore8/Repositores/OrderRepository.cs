using ApiNetCore8.Data;
using ApiNetCore8.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Repositores
{
    public class OrderRepository : IOrderRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddOrderAsync(OrderModel model)
        {
            var newOrder = _mapper.Map<Order>(model);

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return newOrder.OrderId;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var Order = await _context.Orders.FindAsync(id);
            if (Order != null)
            {
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Order not found");
            }
        }

        public async Task<List<OrderModel>> GetAllOrderAsync()
        {
            var Orders = await _context.Orders.ToListAsync();
            return _mapper.Map<List<OrderModel>>(Orders);
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            var Order = await _context.Orders.FindAsync(id);
            return _mapper.Map<OrderModel>(Order);
        }

        public async Task UpdateOrderAsync(int id, OrderModel model)
        {
            var Order = await _context.Orders.FindAsync(id);
            if (Order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            _mapper.Map(model, Order);

            _context.Orders.Update(Order);
            await _context.SaveChangesAsync();
        }
    }
}
