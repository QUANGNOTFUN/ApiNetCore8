using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ApiNetCore8.Data
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        [Range(0, double.MaxValue)]
        [Required] // Đảm bảo TotalAmount là bắt buộc
        public double TotalAmount { get; set; }

        [ForeignKey("Supplier")] // Chỉ định khóa ngoại cho Supplier
        public int SupplierId { get; set; }

        // Navigation Property
        public virtual Supplier Supplier { get; set; }

        // Thêm thuộc tính điều hướng cho OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
