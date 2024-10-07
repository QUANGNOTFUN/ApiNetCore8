using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore8.Data
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        [Required]
        public double UnitPrice { get; set; }

        [ForeignKey("Order")] // Chỉ định khóa ngoại cho Order
        public int OrderId { get; set; }

        [ForeignKey("Product")] // Chỉ định khóa ngoại cho Product
        public int ProductId { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
