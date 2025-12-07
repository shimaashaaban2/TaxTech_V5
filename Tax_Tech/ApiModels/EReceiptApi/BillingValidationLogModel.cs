using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class BillingValidationLogModel
    {
        public long Serial { get; set; }
        public string CUSTCODE { get; set; }
        public string INVOICE_DATE { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string TMCODE { get; set; }
        public string AMOUNT { get; set; }
        public string GROSS_AMOUNT { get; set; }
        public string DISC_AMOUNT { get; set; }
        public string Msg { get; set; }
    }
}

/*
 {
        "Serial": 13872,
        "CUSTCODE": "1.179010255",
        "INVOICE_DATE": "  ",
        "INVOICE_NUMBER": "0000000004012024",
        "TMCODE": "259",
        "AMOUNT": "65",
        "GROSS_AMOUNT": "80",
        "DISC_AMOUNT": null,
        "Msg": "INVOICE_DATE, INVOICE_NUMBER, CUSTCODE, TMCODE, AMOUNT Are Reuired Fields"
    }
 */