using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiNetCore8.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [MaxLength(100)]
        public string CategoryName { get; set; }

        public string? Description { get; set; }

        public List<SupplierInfo> Supplier { get; set; }
    }

    public class SupplierInfo
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }

     // Dùng cho update sản phẩm
     public class CategoryUpdateModel
    {
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public List<int> SupplierIdNew { get; set; }
    }
}
