using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class ItemsTotals
    {
        public string ItemName { get; set; }
        public decimal? DocNo { get; set; }
        public decimal? TaxTotal { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}