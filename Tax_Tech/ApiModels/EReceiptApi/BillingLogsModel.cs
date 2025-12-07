using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class BillingLogsModel
    {
        public long? LogID { get; set; }
        public bool? RequestSuccess { get; set; }
        public string RequestDatetime { get; set; }
        public string Doing { get; set; }
        public long? CountOfReceipt { get; set; }
        public string ResultMsg { get; set; }
        public string LogDatetime { get; set; }
        public string EReceiptNumber { get; set; }
    }
}