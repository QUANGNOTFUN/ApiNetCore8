using System.ComponentModel.DataAnnotations;

public class ProductData
{
    public int ProductID { get; set; }

    [Required]
    [MaxLength(255)]
    public string ProductName { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int ReorderLevel { get; set; }

    [Required]
    public int CategoryID { get; set; }
}
