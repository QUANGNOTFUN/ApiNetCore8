using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
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

        public async Task<PagedResult<OrderModel>> FindOrderAsync(string name, int page, int pageSize)
        {
            // Đếm tổng số danh mục có tên chứa ký tự 'name'
            var totalOrder = await _context.Orders
                .Where(c => c.OrderName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .CountAsync();

            // Lấy danh mục theo tên với phân trang
            var Orders = await _context.Orders
                .Where(c => c.OrderName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
            .ToListAsync();

            var orderModels = _mapper.Map<List<OrderModel>>(Orders);

            return new PagedResult<OrderModel>
            {
                Items = orderModels,
                TotalCount = totalOrder,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<PagedResult<OrderModel>> GetAllOrderAsync(int page, int pageSize)
        {
            var totalOrder = await _context.Orders.CountAsync(); // Đếm tổng số danh mục
            var Orders = await _context.Orders
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            var orderModels = _mapper.Map<List<OrderModel>>(Orders);

            return new PagedResult<OrderModel>
            {
                Items = orderModels,
                TotalCount = totalOrder,
                PageSize = pageSize,
                CurrentPage = page
            };
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
