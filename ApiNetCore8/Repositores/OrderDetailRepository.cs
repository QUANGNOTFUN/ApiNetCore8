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

        // Thêm OrderDetail mới
        public async Task<int> AddOrderDetailAsync(OrderDetailModel model)
        {
            var newOrderDetail = _mapper.Map<OrderDetail>(model);

            // Kiểm tra xem Order và Product có tồn tại hay không
            var existingOrder = await _context.Orders
                .FirstOrDefaultAsync(e => e.OrderId == newOrderDetail.OrderId);
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(e => e.ProductID == newOrderDetail.ProductId);

            if (existingOrder == null || existingProduct == null)
            {
                throw new ArgumentException("OrderId hoặc ProductId không tồn tại.");
            }

            // Kiểm tra OrderDetail có trùng lặp không
            var existingOrderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(e => e.OrderId == newOrderDetail.OrderId && e.ProductId == newOrderDetail.ProductId);

            if (existingOrderDetail != null)
            {
                throw new ArgumentException("OrderDetail cho sản phẩm này đã tồn tại.");
            }

            // Thêm OrderDetail mới
            await _context.OrderDetails.AddAsync(newOrderDetail);
            await _context.SaveChangesAsync();

            return newOrderDetail.OrderDetailId;
        }

        // Xóa OrderDetail
        public async Task DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException("OrderDetail không tìm thấy.");
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }

        public Task FindOrderDetailsAsync(int id, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        // Lấy tất cả OrderDetail
        public async Task<List<OrderDetailModel>> GetAllOrderDetailAsync()
        {
            var orderDetails = await _context.OrderDetails.ToListAsync();
            return _mapper.Map<List<OrderDetailModel>>(orderDetails);
        }

        public Task<List<OrderDetailModel>> GetAllOrderDetailAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        // Lấy OrderDetail theo ID
        public async Task<OrderDetailModel> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException("OrderDetail không tìm thấy.");
            }

            return _mapper.Map<OrderDetailModel>(orderDetail);
        }

        // Cập nhật OrderDetail
        public async Task UpdateOrderDetailAsync(int id, OrderDetailModel model)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException("OrderDetail không tìm thấy.");
            }

            _mapper.Map(model, orderDetail);
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
