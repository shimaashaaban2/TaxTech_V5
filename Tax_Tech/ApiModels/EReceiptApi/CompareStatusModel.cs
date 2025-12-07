using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class CompareStatusModel
    {
        public string Status { get; set; }
        public int Wincash { get; set; }
        public int Billing { get; set; }
    }
}