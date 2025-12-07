using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class ItemsTaxViewModel
    {
        public int? ItemID { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectTaxType", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int? TaxID { get; set; }
        public long? ActionBy { get; set; } = 1;
        public int? RecordID { get; set; }
        public string TaxCode { get; set; }
        public string TaxTypeNameEN { get; set; }
        public int? SubTaxID { get; set; }
        public string SubTaxCode { get; set; }
        public string SubTaxName { get; set; }

        [JsonProperty("rate")]
        public double? Rate { get; set; }
        public bool? IsActive { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectSubTaxType", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int? TaxSubTypeID { get; set; }
    }
}
