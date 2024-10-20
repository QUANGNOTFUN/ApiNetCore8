using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using ApiNetCore8.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace ApiNetCore8.Repositories
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

        // Thêm mới đơn hàng
        public async Task<int> AddOrderAsync(OrderModel model)
        {
            model.OrderId = 0; // Reset OrderId để tránh lỗi auto key

            var newOrder = _mapper.Map<Order>(model); // Mapping OrderModel sang Order entity

            // Kiểm tra xem đơn hàng đã tồn tại chưa
            var existingOrder = await _context.Orders
                .FirstOrDefaultAsync(e => e.OrderDate == newOrder.OrderDate && e.SupplierId == newOrder.SupplierId);

            if (existingOrder != null)
            {
                throw new ArgumentException("Đơn hàng đã tồn tại!");
            }

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return newOrder.OrderId;
        }

        // Kiểm tra và cập nhật lại model nếu null
        public async Task<OrderModel> CheckNullModelAsync(int id, OrderModel model)
        {
            var existingOrder = await GetOrderByIdAsync(id);

            if (model.OrderDate == default(DateTime))
            {
                model.OrderDate = existingOrder.OrderDate;
            }

            if (model.SupplierId == 0)
            {
                model.SupplierId = existingOrder.SupplierId;
            }

            return model;
        }

        // Xóa đơn hàng
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(c => c.OrderId == id); // Tìm đơn hàng theo ID

            if (order == null)
            {
                throw new KeyNotFoundException("Không có đơn hàng tồn tại");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        // Lấy tất cả đơn hàng
        public async Task<PagedResult<OrderModel>> GetAllOrderAsync(int page, int pageSize)
        {
            var totalOrders = await _context.Orders.CountAsync(); // Đếm tổng số đơn hàng
            var orders = await _context.Orders
                .Skip((page - 1) * pageSize) // Bỏ qua các đơn hàng ở các trang trước
                .Take(pageSize) // Lấy số đơn hàng trong trang hiện tại
                .ToListAsync();

            var orderModels = _mapper.Map<List<OrderModel>>(orders);

            return new PagedResult<OrderModel>
            {
                Items = orderModels,
                TotalCount = totalOrders,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        // Tìm kiếm đơn hàng theo ID hoặc ngày đơn hàng
        public async Task<PagedResult<OrderModel>> FindOrdersAsync(int? orderId, DateTime? orderDate, int page, int pageSize)
        {
            var query = _context.Orders.AsQueryable();

            if (orderId.HasValue)
            {
                query = query.Where(o => o.OrderId == orderId.Value);
            }

            if (orderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date == orderDate.Value.Date);
            }

            var totalOrders = await query.CountAsync();
            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var orderModels = _mapper.Map<List<OrderModel>>(orders);

            return new PagedResult<OrderModel>
            {
                Items = orderModels,
                TotalCount = totalOrders,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        // Lấy đơn hàng theo Id
        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(c => c.OrderId == id);

            if (order == null)
            {
                throw new KeyNotFoundException("Không có đơn hàng tồn tại");
            }
            return _mapper.Map<OrderModel>(order);
        }

        // Cập nhật lại đơn hàng
        public async Task UpdateOrderAsync(int id, OrderModel model)
        {
            model = await CheckNullModelAsync(id, model);

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Không có Id đơn hàng");
            }

            model.OrderId = id; // Gắn id truyền vào form nếu không nhập

            _mapper.Map(model, order);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public Task<PagedResult<OrderModel>> FindCategoriesAsync(string name, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<OrderModel>> FindOrdersAsync(int id, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
