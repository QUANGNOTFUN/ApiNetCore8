using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore8.Data
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [MaxLength(100)]
        public required string CategoryName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
