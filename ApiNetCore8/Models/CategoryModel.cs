using System.ComponentModel.DataAnnotations;

namespace ApiNetCore8.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [MaxLength(100)]
        public string CategoryName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
