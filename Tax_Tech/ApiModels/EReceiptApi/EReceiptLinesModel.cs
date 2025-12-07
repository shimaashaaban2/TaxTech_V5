using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class EReceiptLinesModel
    {
        public string description { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string UnitType { get; set; }
        public double? quantity { get; set; }
        public string InternalCode { get; set; }
        public double? SalesTotal { get; set; }
        public double? TotalOld { get; set; }
        public double? ItemsDiscountOld { get; set; }
        public double? AmountEGP { get; set; }
        public double? amountSold { get; set; }
        public double? currencyExchangeRate { get; set; }
        public long? LineID { get; set; }
        public long? EReceiptID { get; set; }
        public long? ItemID { get; set; }
        public int? currencySold { get; set; }
        public string ItemName { get; set; }
        public double? ItemTaxTotalB { get; set; }
        public string LineDesc { get; set; }
        public string UnitTypeold { get; set; }
        public double? ItemTaxTotal { get; set; }
        public double? DiscountAmount { get; set; }
        public double? ItemsDiscount { get; set; }
        public string DescOld { get; set; }
        public double? Total { get; set; }
        public double? netTotal { get; set; }
        public double? ValueDifference { get; set; }
        public double? DiscountRate { get; set; }
    }
}