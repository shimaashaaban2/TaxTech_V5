using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class InvoiceLineCreateViewModel
    {

        public long? InvoiceInternalID { get; set; }
        public long? LineID { get; set; }
        public long? ActionBy { get; set; }

        [Required(ErrorMessageResourceName = "ItemCannotbeKeptEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int? ItemID { get; set; }
        public string ItemERPID { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        [Required(ErrorMessageResourceName = "QuantityCannotbeKeptEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "QuantityValueBiggerThan0", ErrorMessageResourceType = typeof(Resources.Resource))]
        public double? Quantity { get; set; }


        [Required(ErrorMessageResourceName = "CurrencyCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Currency { get; set; }

        [Required(ErrorMessageResourceName = "AmountCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "AmountBiggerThan0", ErrorMessageResourceType = typeof(Resources.Resource))]
        public double? Amount { get; set; }

        [JsonProperty("Amount")]
        [Required(ErrorMessageResourceName = "AmountCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public double? SoldTotal { get; set; }

        [Required(ErrorMessageResourceName = "ExchangeRateCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "EnterExchangeRateLargerThan0", ErrorMessageResourceType = typeof(Resources.Resource))]
        public double? ExchangeRate { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "DiscountMustBeLargerThan0", ErrorMessageResourceType = typeof(Resources.Resource))]
        public double? Discount { get; set; }

        public bool percentageDiscount { get; set; }
        public int? ReturnPartial { get; set; } = 0;
    }
}
