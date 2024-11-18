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
        public string OrderName { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
