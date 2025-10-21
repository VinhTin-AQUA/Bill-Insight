using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using BillInsight.Models.InvoiceDetails;

namespace BillInsight.Controls
{
    public partial class InvoiceDetailsItem : UserControl
    {
        #region properties

        public static readonly StyledProperty<List<InvoiceItem>> ProductsProperty =
            AvaloniaProperty.Register<InvoiceDetailsItem, List<InvoiceItem>>(nameof(Products), []);

        public List<InvoiceItem> Products
        {
            get => GetValue(ProductsProperty);
            set => SetValue(ProductsProperty, value);
        }
        
        #endregion
        
        public InvoiceDetailsItem()
        {
            InitializeComponent();
        }
    }
}