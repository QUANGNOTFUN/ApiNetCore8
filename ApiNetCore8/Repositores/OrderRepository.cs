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
            // Tạo đối tượng Order mới
            var newOrder = new Order
            {
                OrderDate = DateTime.Now,
                SupplierId = model.SupplierId,
                OrderName = button == "Đặt hàng" ? "Đơn đặt hàng" : "Đơn xuất hàng",
                Status = "Pending",
                TotalPrice = 0,
                OrderDetails = new List<OrderDetail>() // Khởi tạo danh sách chi tiết đơn hàng
            };

            decimal totalPrice = 0; // Tổng giá trị đơn hàng

            // Thêm chi tiết đơn hàng
            if (model.addOrderDetails != null && model.addOrderDetails.Any())
            {
                foreach (var detail in model.addOrderDetails)
                {
                    // Lấy thông tin sản phẩm từ ProductId
                    var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == detail.ProductId);

                    if (product == null)
                    {
                        throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {detail.ProductId}");
                    }

                    // Tính giá đơn vị dựa vào loại đơn hàng
                    decimal unitPrice = (button == "Đặt hàng")
                        ? product.CostPrice * detail.Quantity
                        : product.SellPrice * detail.Quantity;
                    if (unitPrice == 0)
                    {
                        throw new InvalidOperationException("Tổng đơn giá chi tiết = 0");
                    }
                    // Thêm chi tiết đơn hàng
                    var newDetail = new OrderDetail
                    {
                        OrderDetailName = product.ProductName,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = unitPrice,
                        OrderId = newOrder.OrderId
                    };

                    // Thêm vào danh sách chi tiết đơn hàng của Order
                    newOrder.OrderDetails.Add(newDetail);
                    _context.OrderDetails.Add(newDetail);

                    // Cộng vào tổng giá trị của đơn hàng
                    totalPrice += unitPrice;
                }
            }

            // Cập nhật tổng giá trị vào đơn hàng
            newOrder.TotalPrice = totalPrice;

            // Lưu đơn hàng và chi tiết vào cơ sở dữ liệu
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
                .Include(o => o.OrderDetails)
                    .Skip((page - 1) * pageSize) // Bỏ qua các danh mục ở các trang trước
                    .Take(pageSize) // Lấy số danh mục trong trang hiện tại
                    .ToListAsync();

            var orderModels = Orders.Select(order => new OrderModel
            {
                OrderId = order.OrderId,
                OrderName = order.OrderName,
                OrderDate = order.OrderDate,       
                SupplierId = order.SupplierId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                OrderDetailsId = order.OrderDetails.Select(orDT => orDT.OrderDetailId).ToList() ?? new List<int>()
            }).ToList();

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
            .Include(o => o.OrderDetails)      
            .ThenInclude(od => od.Product)
            .SingleOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");
            }

            var orderModel = new OrderModel
            {
                OrderId = order.OrderId,
                OrderName = order.OrderName,
                OrderDate = order.OrderDate,
                SupplierId = order.SupplierId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                OrderDetailsId = order.OrderDetails.Select(orDT => orDT.OrderDetailId).ToList() ?? new List<int>()
            };

            return orderModel;
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
            // Tìm đơn hàng theo ID
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");
            }

            // Cập nhật trạng thái đơn hàng
            order.Status = status;

            // Nếu xác nhận đơn hàng, xử lý tồn kho và tính toán tổng giá trị đơn hàng
            if (action == "confirm" && status == "Successful")
            {
                int totalPrice = 0;

                foreach (var detail in order.OrderDetails)
                {
                    var product = detail.Product;

                    if (product == null)
                    {
                        throw new KeyNotFoundException($"Sản phẩm với ID {detail.ProductId} không tồn tại.");
                    }

                    // Xử lý nhập hàng
                    if (order.OrderName == "Đơn đặt hàng")
                    {
                        product.StockQuantity += detail.Quantity;
                    }
                    // Xử lý xuất hàng
                    else if (order.OrderName == "Đơn xuất hàng")
                    {
                        if (product.StockQuantity < detail.Quantity)
                        {
                            throw new InvalidOperationException($"Không đủ hàng trong kho cho sản phẩm ID {detail.ProductId}.");
                        }
                        product.StockQuantity -= detail.Quantity;
                    }

                    // Tính tổng giá trị của đơn hàng
                    totalPrice += (int)(detail.UnitPrice * detail.Quantity);
                }

                // Cập nhật tổng giá trị vào đơn hàng
                order.TotalPrice = totalPrice;
            }

            // Lưu thay đổi
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

    }
}
