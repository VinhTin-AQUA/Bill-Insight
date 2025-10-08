using System;
using System.Collections.ObjectModel;
using BillInsight.Models.Products;

namespace BillInsight.ViewModels
{
    public class InvoiceDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<Invoice> Products { get; set; } = 
        [
            new()
            {
                DatePurchased = DateTime.Now,
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-1),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-2),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() {  Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now,
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-1),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-2),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() {  Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now,
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-1),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-2),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() {  Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now,
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-1),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-2),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() {  Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now,
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-1),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-2),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() {  Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now,
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-1),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-2),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() {  Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now,
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-1),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() { Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            },
            new()
            {
                DatePurchased = DateTime.Now.AddDays(-2),
                Products = 
                [
                    new() { Name = "Bàn phím cơ", Price = 1200000, PaymentMethod = "VND" },
                    new() {  Name = "Chuột Logitech", Price = 650000, PaymentMethod = "VND" },
                    new() { Name = "Tai nghe Bluetooth", Price = 950000, PaymentMethod = "VND" },                
                ]
            }
        ];

        public InvoiceDetailsViewModel()
        {
        }
        
        public override void Dispose()
        {
            // các lệnh giải phóng tài nguyên
            base.Dispose();
        }
    }
}