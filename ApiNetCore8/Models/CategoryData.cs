using System.ComponentModel.DataAnnotations;

namespace ApiNetCore8.Models
{
    public class CategoryData
    {
        public int CategoryId { get; set; }

        [MaxLength(100)]
        public required string CategoryName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
