using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class FilterByDateModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long? Status { get; set; }
    }
}