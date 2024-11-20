using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiNetCore8.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        public string OrderDetailName { get; set; }

        [JsonIgnore]
        public int OrderId { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
    public class addOrderDetailModel
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
