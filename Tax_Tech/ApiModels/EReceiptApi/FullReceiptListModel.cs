using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class FullReceiptListModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public byte EReceiptSource { get; set; }
        public string EReceiptStatus { get; set; }
    }
}