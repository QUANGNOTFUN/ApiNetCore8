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

        public async Task<int> AddOrderAsync(string button, addOrderModel model)
        {
            // Tạo đơn hàng mới
            var newOrder = new Order
            {
                OrderDate = model.OrderDate,
                SupplierId = model.SupplierId,
                OrderName = button == "Đặt hàng" ? "Đơn đặt hàng" : "Đơn xuất hàng",
                Status = "Pending"
            };

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            // Thêm chi tiết đơn hàng
            if (model.addOrderDetails != null && model.addOrderDetails.Any())
            {
                foreach (var detail in model.addOrderDetails)
                {
                    var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == detail.ProductId);

                    if (product == null)
                    {
                        throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {detail.ProductId}");
                    }

                    // Kiểm tra tồn kho và cập nhật
                    if (button == "Xuất hàng" && product.StockQuantity < detail.Quantity)
                    {
                        throw new InvalidOperationException($"Số lượng tồn kho không đủ cho sản phẩm ID {detail.ProductId}");
                    }

                    if (button == "Đặt hàng")
                    {
                        product.StockQuantity += detail.Quantity;
                        detail.UnitPrice = product.CostPrice;
                    }
                    else if (button == "Xuất hàng")
                    {
                        product.StockQuantity -= detail.Quantity;
                        detail.UnitPrice = product.SellPrice;
                    }

                    var newDetail = new OrderDetail
                    {
                        OrderDetailName = detail.OrderDetailName,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice,
                        OrderId = newOrder.OrderId
                    };

                    await _context.OrderDetails.AddAsync(newDetail);
                }

                await _context.SaveChangesAsync();
            }

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
            var order = await _context.Orders
            .Include(o => o.OrderDetails)       // Bao gồm chi tiết đơn hàng
            .ThenInclude(od => od.Product)
            .SingleOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");
            }

            return _mapper.Map<OrderModel>(order);
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
        public async Task UpdateOrderStatusAndQuantityAsync(int orderId, string status, string action)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");
            }

            // Cập nhật trạng thái
            order.Status = status;

            // Nếu xác nhận đơn hàng, xử lý tồn kho
            if (action == "confirm" && status == "Successful")
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = detail.Product;

                    if (product == null)
                    {
                        throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {detail.ProductId}");
                    }

                    if (order.OrderName == "Đơn đặt hàng")
                    {
                        product.StockQuantity += detail.Quantity;
                    }
                    else if (order.OrderName == "Đơn xuất hàng")
                    {
                        if (product.StockQuantity < detail.Quantity)
                        {
                            throw new InvalidOperationException($"Không đủ hàng trong kho cho sản phẩm ID {detail.ProductId}.");
                        }
                        product.StockQuantity -= detail.Quantity;
                    }
                }
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

    }
}
