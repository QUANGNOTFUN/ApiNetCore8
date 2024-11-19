using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore8.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        public string OrderDetailName { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
    public class addOrderDetailModel
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
