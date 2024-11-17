using System.ComponentModel.DataAnnotations;

public class ProductModel
{
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Hãy nhập tên sản phẩm.")]
    [MaxLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự.")]
    public string ProductName { get; set; }

    [MaxLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Hãy nhập giá sản phẩm.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
    public decimal CostPrice { get; set; }

    [Required(ErrorMessage = "Hãy nhập giá sản phẩm.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
    public decimal SellPrice { get; set; }

    [Required(ErrorMessage = "Hãy nhập số lượng trong kho.")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng trong kho phải lớn hơn hoặc bằng 0.")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "Hãy nhập mức đặt hàng lại.")]
    [Range(0, int.MaxValue, ErrorMessage = "Mức đặt hàng lại phải lớn hơn hoặc bằng 0.")]
    public int ReorderLevel { get; set; }

    [Required(ErrorMessage = "Hãy nhập ID danh mục.")]
    public int CategoryID { get; set; }
}
