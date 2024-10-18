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
       

      
        public async Task<PagedResult<OrderDetailModel>> FindOrderDetailAsync(string name, int page, int pageSize)
        {
            // Đếm tổng số danh mục có tên chứa ký tự 'name'
            var totalOrderDetails = await _context.OrderDetails
                .Where(c => c.OrderDetailName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .CountAsync();

            // Lấy danh mục theo tên với phân trang
            var OrderDetails = await _context.OrderDetails
                .Where(c => c.OrderDetailName.Contains(name)) // Điều kiện tìm kiếm theo tên
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            var OrderDetailModel = _mapper.Map<List<OrderDetailModel>>(OrderDetails);

            return new PagedResult<OrderDetailModel>
            {
                Items = OrderDetailModel,
                TotalCount = totalOrderDetails,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<PagedResult<OrderDetailModel>> GetAllOrderDetailAsync(int page, int pageSize)
        {
            var totalOrderDetails = await _context.OrderDetails.CountAsync(); // Đếm tổng số danh mục
            var OrderDetails = await _context.OrderDetails
                .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                .ToListAsync();

            var OrderDetailModel = _mapper.Map<List<OrderDetailModel>>(OrderDetails);

            return new PagedResult<OrderDetailModel>
            {
                Items =OrderDetailModel,
                TotalCount = totalOrderDetails,
                PageSize = pageSize,
                CurrentPage = page
            };
        
    }}
}
