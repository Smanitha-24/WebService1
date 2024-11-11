namespace SalesTaxService.Models
{
    public class ResponseModel
    {
        public decimal TotalIncludingTax { get; set; }
        public decimal SalesTax { get; set; }
        public decimal TotalExcludingTax { get; set; }
        public string CostCentre { get; set; } = string.Empty;
        public decimal TaxRate { get; set; } = 0.1m;
        public string Message { get; set; } = string.Empty;
    }
}
