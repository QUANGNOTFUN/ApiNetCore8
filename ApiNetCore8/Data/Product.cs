using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore8.Data
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(255)]
        public required string ProductName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellPrice { get; set; }

        [Required]
        public int StockQuantity { get; set; } // Số lượng trong kho

        [Required]
        public int ReorderLevel { get; set; } // Mức cảnh báo

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public virtual Category? Category { get; set; }

        // Khởi tạo tập hợp OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
