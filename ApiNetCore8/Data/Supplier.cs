using System.Collections.Generic;
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
        [MaxLength(100)]
        public required string ContactInfo { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
