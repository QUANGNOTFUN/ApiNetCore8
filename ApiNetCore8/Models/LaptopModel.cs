using System.ComponentModel.DataAnnotations;

namespace ApiNetCore8.Models
{
    public class LaptopModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        [Range(0, double.MaxValue)]
        public int Quantity { get; set; }
    }
}
