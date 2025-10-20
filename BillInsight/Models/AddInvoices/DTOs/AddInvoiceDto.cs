namespace BillInsight.Models.AddInvoices.DTOs
{
    public class AddInvoiceDto
    {
        public string Date { get; set; } = "";
        public string ItemName { get; set; } = "";
        public string Cash { get; set; } = "";
        public string Bank { get; set; } = "";
    }
}