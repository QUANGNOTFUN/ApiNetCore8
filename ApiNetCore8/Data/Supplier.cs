using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore8.Data
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string SupplierName { get; set; }

        [Required]
        [MaxLength(20)]
        public required string ContactInfo { get; set; }

        // Định nghĩa CategoryId là khóa ngoại
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Thêm thuộc tính Category để thiết lập quan hệ
        public virtual Category Category { get; set; }
        public virtual Order Order { get; set; }
    }
}
