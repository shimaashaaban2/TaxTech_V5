using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class EReceiptTrackingViewModel
    {
        public IEnumerable<EReceiptTrackingModel> eReceiptTrackings { get; set; }
        public IEnumerable<BillingTrackingModel> billingTrackings { get; set; }
    }
}