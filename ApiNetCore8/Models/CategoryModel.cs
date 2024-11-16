using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiNetCore8.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [MaxLength(100)]
        public string CategoryName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        // Thay đổi thành danh sách các SupplierId và SupplierName
        public List<SupplierInfo> Supplier { get; set; }
    }

    public class SupplierInfo
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
