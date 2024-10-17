using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public OrderDetailRepository(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddOrderDetailAsync(OrderDetailModel model)
        {
            var newOrderDetail = _mapper.Map<OrderDetail>(model);

            var existingOrder = await _context.Orders.FirstOrDefaultAsync(e => e.OrderId == newOrderDetail.OrderId);

            var existingProduct = await _context.Products.FirstOrDefaultAsync(e => e.ProductID == newOrderDetail.ProductId);

            var existingOrderDetail = await _context.OrderDetails
                                                 .FirstOrDefaultAsync(e => e.ProductId == newOrderDetail.ProductId);
            if (existingOrder == null || existingProduct == null)
            {
                throw new ArgumentException("ProductId hoặc OrderId chưa tồn tại"); 
            }
            if (existingOrderDetail != null)
            {
                throw new ArgumentException("OrderDetailName already exists.");
            } 

            await _context.OrderDetails.AddAsync(newOrderDetail);
            await _context.SaveChangesAsync();

            return newOrderDetail.OrderDetailId;
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            var OrderDetail = await _context.OrderDetails.FindAsync(id);
            if (OrderDetail != null)
            {
                _context.OrderDetails.Remove(OrderDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("OrderDetail not found");
            }
        }

        public async Task<List<OrderDetailModel>> GetAllOrderDetailAsync()
        {
            var OrderDetails = await _context.OrderDetails.ToListAsync();
            return _mapper.Map<List<OrderDetailModel>>(OrderDetails);
        }

        public async Task<OrderDetailModel> GetOrderDetailByIdAsync(int id)
        {
            var OrderDetail = await _context.OrderDetails.FindAsync(id);
            return _mapper.Map<OrderDetailModel>(OrderDetail);
        }

        public async Task UpdateOrderDetailAsync(int id, OrderDetailModel model)
        {
            var OrderDetail = await _context.OrderDetails.FindAsync(id);
            if (OrderDetail == null)
            {
                throw new KeyNotFoundException("OrderDetail not found");
            }

            _mapper.Map(model, OrderDetail);

            _context.OrderDetails.Update(OrderDetail);
            await _context.SaveChangesAsync();
        }
        public async Task<List<OrderDetailModel>> SearchOrderDetailsAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Giả sử tìm kiếm theo ProductName
                query = query.Where(od => od.Product.ProductName.Contains(searchTerm));
            }

            var orderDetails = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<OrderDetailModel>>(orderDetails);
        }

        // Thêm phương thức lấy 20 sản phẩm
        public async Task<List<OrderDetailModel>> GetLimitedOrderDetailsAsync(int limit)
        {
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .Take(limit)
                .ToListAsync();

            return _mapper.Map<List<OrderDetailModel>>(orderDetails);
        }
    }
}
