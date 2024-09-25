using System.ComponentModel.DataAnnotations;
using ApiNetCore8.Models;
namespace ApiNetCore8.Models
{
    public class Supplier
    {
        public int Id { get; set; } // Khóa chính, tự động tăng
        public int IdPC { get; set; } // Khóa ngoại đến ProductCategories

        [Required]
        public string ContactInfo { get; set; } // Thông tin liên lạc

       //danh mục sản phẩm
        public ProductCategory ProductCategory { get; set; }
    }
    public class ProductCategory: Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }

    }

}
