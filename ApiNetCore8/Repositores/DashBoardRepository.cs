using ApiNetCore8.Data;
using ApiNetCore8.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Repositores
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly InventoryContext _context;

        public DashBoardRepository(InventoryContext context) 
        {
            _context = context;
        }

        public async Task<DashBoardModel> GetDashBoardInDayAsync()
        {
            // Lấy ngày hiện tại
            var today = DateTime.Today;

            // Lọc các đơn hàng có DateTime trong ngày hiện tại
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Date == today) // So sánh chỉ phần ngày
                .ToListAsync();

            var dashBoard = new DashBoardModel();
            dashBoard.DateTime = DateTime.Now;

            foreach (var order in orders)
            {
                if (order.Status == "Successful")
                {
                    dashBoard.TotalPriceIn += order.OrderName == "Đơn đặt hàng" ? order.TotalPrice : 0;

                    dashBoard.TotalPriceOut += order.OrderName == "Đơn xuất hàng" ? order.TotalPrice : 0;

                    foreach (var orderDt in order.OrderDetails)
                    {
                        dashBoard.QuantityProductIn += order.OrderName == "Đơn đặt hàng" ? orderDt.Quantity : 0;

                        dashBoard.QuantityProductOut += order.OrderName == "Đơn xuất hàng" ? orderDt.Quantity : 0;
                    }
                }
            }

            return dashBoard;
        }


        public async Task<DashBoardModel> GetDashBoardAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ToListAsync();
            

            var dashBoard = new DashBoardModel();
            dashBoard.DateTime = DateTime.Now;

            foreach (var order in orders)
            {
                if (order.Status == "Successful")
                {
                    dashBoard.TotalPriceIn += order.OrderName == "Đơn đặt hàng" ? order.TotalPrice : 0;

                    dashBoard.TotalPriceOut += order.OrderName == "Đơn xuất hàng" ? order.TotalPrice : 0;

                    foreach (var orderDt in order.OrderDetails )
                    {
                        dashBoard.QuantityProductIn += order.OrderName == "Đơn đặt hàng" ? orderDt.Quantity : 0;

                        dashBoard.QuantityProductOut += order.OrderName == "Đơn xuất hàng" ? orderDt.Quantity : 0;
                    }
                }
            }

            return dashBoard;
        }

        public async Task<DashBoardSummaryModel> GetSummaryAsync()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalSuppliers = await _context.Suppliers.CountAsync();

            return new DashBoardSummaryModel
            {
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalSuppliers = totalSuppliers
            };
        }
    }
}
