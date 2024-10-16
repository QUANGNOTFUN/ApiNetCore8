namespace ApiNetCore8.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } // Danh sách sản phẩm
        public int TotalCount { get; set; } // Tổng số sản phẩm
        public int PageSize { get; set; } // Kích thước trang
        public int CurrentPage { get; set; } // Trang hiện tại
    }
}
