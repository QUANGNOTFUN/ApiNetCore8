﻿using ApiNetCore8.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore8.Models
{
    public class OrderModel
    {

        public int OrderId { get; set; }
        public string OrderName { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; } 
        public List<ProductModel> ProductModels { get; set; }
    }
}
