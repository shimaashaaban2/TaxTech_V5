using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class ReceiptLinesTotals
    {
        public long? RecordID { get; set; }
        public long? EReceiptID { get; set; }
        public double? totalSalesAmount { get; set; }
        public double? totalItemsDiscountAmount { get; set; }
        public double? netAmount { get; set; }
        public double? taxTotals { get; set; }
        public double? extraDiscountAmount { get; set; }
        public double? totalDiscountAmount { get; set; }
        public double? totalAmount { get; set; }
      
    }
}
