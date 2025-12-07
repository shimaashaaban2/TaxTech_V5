using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class TaxType
    {
        public int? TaxTypeID { get; set; }
        public string TaxCode { get; set; }
        public string TaxTypeNameEN { get; set; }
        public string TaxTypeNameAR { get; set; }
        public int? TaxParentID { get; set; }
        
        [JsonProperty("rate")]
        public string Rate { get; set; }
    }
}
