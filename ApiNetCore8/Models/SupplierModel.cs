using System.ComponentModel.DataAnnotations;

namespace ApiNetCore8.Models
{
    public class SupplierModel
    {
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string SupplierName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string ContactInfo { get; set; }
    }
}
