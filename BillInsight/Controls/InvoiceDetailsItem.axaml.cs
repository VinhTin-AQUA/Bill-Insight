using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            
            // Theo dõi thay đổi property Products
            this.GetObservable(ProductsProperty).Subscribe(_ => UpdateTotals());
        }
        
        private void UpdateTotals()
        {
            if (Products.Count == 0)
            {
                TotalCashText.Text = "0";
                TotalBankText.Text = "0";
                return;
            }

            decimal totalCash = Products.Sum(p => ParseStringToDecimal(p.Cash));
            decimal totalBank = Products.Sum(p => ParseStringToDecimal(p.Bank));

            TotalCashText.Text = totalCash.ToString("N2");
            TotalBankText.Text = totalBank.ToString("N2");
        }

        private decimal ParseStringToDecimal(string input)
        {
            string cleaned = input.Replace("đ", "")
                .Replace("₫", "")
                .Trim();

            cleaned = cleaned.Replace(".", "");

            cleaned = cleaned.Replace(",", ".");
            var check = decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value);
            return check ? value : -1;
        }
    }
}