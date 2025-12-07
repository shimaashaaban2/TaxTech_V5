using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class TotalReportsModel
    {
        public string Source { get; set; }
        public string receiptType { get; set; }
        public DateTime? dateTimeIssued { get; set; }
        public decimal? SalesTotal { get; set; }
        public decimal? ItemsDiscount { get; set; }
        public decimal? NetTotal { get; set; }
        public decimal? TaxTotals { get; set; }
        public decimal? extraDiscountAmount { get; set; }
    }
}