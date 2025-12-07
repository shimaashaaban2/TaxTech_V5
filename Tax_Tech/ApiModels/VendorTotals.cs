using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class VendorTotals
    {
        public int? VendorID { get; set; }
        public string VendorName { get; set; }
        public int? DocCount { get; set; }
        public double? TaxSum { get; set; }
        public double? TotalAmount { get; set; }
    }
}
