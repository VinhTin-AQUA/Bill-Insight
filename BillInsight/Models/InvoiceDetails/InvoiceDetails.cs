using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace BillInsight.Models.InvoiceDetails
{
    public class InvoiceDetails : ReactiveObject
    {
        private string _datePurchased = "";
        public string DatePurchased
        {
            get => _datePurchased; 
            set =>  this.RaiseAndSetIfChanged(ref _datePurchased, value);
        } 
        
        private List<InvoiceItem> _items= [];
        public List<InvoiceItem> Items
        {
            get => _items;
            set => this.RaiseAndSetIfChanged(ref _items, value);
        }
    }

    public class InvoiceItem
    {
        public string ItemName { get; set; } = string.Empty;
        public string Cash { get; set; } = "";
        public string Bank { get; set; } = string.Empty;
    }
}