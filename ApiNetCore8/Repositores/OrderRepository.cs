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
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Order not found");
            }
        }

        public async Task<List<OrderModel>> GetAllOrderAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Supplier) // Bao gồm thông tin nhà cung cấp
                .ToListAsync();

            return _mapper.Map<List<OrderModel>>(orders);
        }

        public async Task<List<OrderModel>> GetLimitedOrdersAsync(int limit)
        {
            var orders = await _context.Orders
                .Include(o => o.Supplier) // Bao gồm thông tin nhà cung cấp
                .Take(limit)
                .ToListAsync();

            return _mapper.Map<List<OrderModel>>(orders);
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Supplier) // Bao gồm thông tin nhà cung cấp
                .FirstOrDefaultAsync(o => o.OrderId == id);

            return _mapper.Map<OrderModel>(order);
        }

        // Phương thức tìm kiếm với phân trang
        public async Task<List<OrderModel>> SearchOrdersAsync(DateTime? startDate, DateTime? endDate, int page, int pageSize)
        {
            var query = _context.Orders.Include(o => o.Supplier).AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= endDate.Value);
            }

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<OrderModel>>(orders);
        }

        public Task<List<OrderModel>> SearchOrdersAsync(string searchTerm, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderAsync(int id, OrderModel model)
        {
            throw new NotImplementedException();
        }
    }
}
