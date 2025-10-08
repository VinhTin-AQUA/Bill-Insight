using System;
using System.Collections.Generic;

namespace BillInsight.Models.Products
{
    public class ProductModel
    {
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class Invoice
    {
        public DateTime DatePurchased { get; set; }
        public List<ProductModel> Products { get; set; } = [];
    }
}