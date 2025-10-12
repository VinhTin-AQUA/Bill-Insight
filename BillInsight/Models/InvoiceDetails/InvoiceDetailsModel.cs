using System;
using System.Collections.Generic;

namespace BillInsight.Models.InvoiceDetails
{
    public class InvoiceDetailsModel
    {
        public DateTime DatePurchased { get; set; }
        public List<ProductItem> Products { get; set; } = [];
    }

    public class ProductItem
    {
        public string ProductName { get; set; } = string.Empty;
        public float ProductPrice { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}