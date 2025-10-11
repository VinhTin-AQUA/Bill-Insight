using System;
using System.Collections.Generic;

namespace BillInsight.Models.Products
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string PaymentMethod { get; set; }

        public ProductModel(string name, float price, string paymentMethod)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Price = price;
            PaymentMethod = paymentMethod;
        }
    }

    public class Invoice
    {
        public DateTime DatePurchased { get; set; }
        public List<ProductModel> Products { get; set; } = [];
    }
}