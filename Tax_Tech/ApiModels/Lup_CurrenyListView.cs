using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class Lup_CurrenyListView
    {
        public long CurrencyIndex { get; set; }
        public string CurrencyTitle { get; set; }
        public string CurrencyCode { get; set; }
        public bool? IsActive { get; set; }
    }
}