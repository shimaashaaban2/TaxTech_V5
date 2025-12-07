using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class VendorTaxViewModel
    {
        public long? RecordID { get; set; }
        public long? VendorID { get; set; }
        public string TaxTypeNameEN { get; set; }
        public double? Rate { get; set; }
        public string TaxCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
