using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class DocumentTotal
    {
        public long? DocumentID { get; set; }
        public double? SalesTotal { get; set; }
        public double? ItemsDiscount { get; set; }
        public double? NetTotal { get; set; }
        public double? TaxTotals { get; set; }
        public double? TotalAmount { get; set; }

        [JsonProperty("extraDiscountAmount")]
        public double? ExtraDiscountAmount { get; set; }
    }
}
