using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.ViewModels
{
    public class PriceDetailsViewModel
    {
        public long ItemSerial { get; set; }
        public long ItemID { get; set; }
        public decimal NetPrice { get; set; }
        public string SwitchDate { get; set; }
        public bool IsActive { get; set; }
        public long ActionBy { get; set; }
        public string DisplayName { get; set; }
        public decimal? BasicPrice { get; set; }
        public decimal? TaxT2 { get; set; }
        public decimal? SellingDistributionExpenses { get; set; }
        public decimal? TaxT3 { get; set; }
        public decimal? HealtInsuranceMI02 { get; set; }
        public decimal? HealtInsuranceMI04 { get; set; }
    }
}