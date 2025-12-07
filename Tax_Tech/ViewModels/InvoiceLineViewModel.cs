using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class InvoiceLineViewModel
    {
        
        public long? InvoiceInternalID { get; set; }
        public int? LineID { get; set; }
        public int? DocumentID { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemERPID { get; set; }
        public string ItemCode { get; set; }
        public string CurrencyCode { get; set; }

        public double? UnitInEGP { get; set; }
        public double? ItemTaxTotal { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        [JsonProperty("currencySold")]
        public int? CurrencySold { get; set; }

        [JsonProperty("amountSold")]
        public double? AmountSold { get; set; }

        [JsonProperty("currencyExchangeRate")]
        public double? CurrencyExchangeRate { get; set; }

        [JsonProperty("UnitValueamountEGP")]
        public double? UnitValueAmountEGP { get; set; }
        
        [JsonProperty("salesTotal")]
        public double? SalesTotal { get; set; }

        public double? ItemsDiscount { get; set; }

        [JsonProperty("nettotal")]
        public double? NetTotal { get; set; }

        [JsonProperty("totalTaxableFees")]
        public double? TotalTaxableFees { get; set; }

        [JsonProperty("valueDifference")]
        public double? ValueDifference { get; set; }

        [JsonProperty("total")]
        public double? Total { get; set; }
    }
}
