using ApiNetCore8.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    public int ProductID { get; set; }

    [Required]
    [MaxLength(255)]
    public string ProductName { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    [Required]
    public int ReorderLevel { get; set; }

    [ForeignKey("Category")]
    public int CategoryID { get; set; }

    [ForeignKey("OrderDetail")]
    public int OrderDetailID { get; set; }

    public virtual Category Category { get; set; }
    public virtual OrderDetail OrderDetail { get; set; }
}
