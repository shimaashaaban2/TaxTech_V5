using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class BillingTrackingModel
    {
        public string INVOICE_NUMBER { get; set; }
        public string Msg { get; set; }
        public string BillingStatus { get; set; }
        public bool IsValid { get; set; }
        public string CUSTCODE { get; set; }
        public string TMCODE { get; set; }
        public string AMOUNT { get; set; }
        public string INVOICE_DATE { get; set; }
    }
    
        
}