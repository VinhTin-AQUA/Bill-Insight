using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using BillInsight.Models.Products;

namespace BillInsight.Controls
{
    public partial class InvoiceDetailsItem : UserControl
    {
        #region properties

        public static readonly StyledProperty<List<ProductModel>> ProductsProperty =
            AvaloniaProperty.Register<InvoiceDetailsItem, List<ProductModel>>(nameof(Products), []);

        public List<ProductModel> Products
        {
            get => GetValue(ProductsProperty);
            set => SetValue(ProductsProperty, value);
        }

        public static readonly StyledProperty<DateTime> DatePurchasedProperty =
            AvaloniaProperty.Register<InvoiceDetailsItem, DateTime>(nameof(DatePurchased), DateTime.MinValue);

        public DateTime DatePurchased
        {
            get => GetValue(DatePurchasedProperty);
            set => SetValue(DatePurchasedProperty, value);
        }
        
        #endregion
        
        public InvoiceDetailsItem()
        {
            InitializeComponent();
        }
    }
}