using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class ResultSummeryModel
    {
        public string EReceiptStatus { get; set; }
        public string IsWaiting { get; set; }
        public int? CountORecored { get; set; }
         
    }
}