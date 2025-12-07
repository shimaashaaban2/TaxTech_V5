using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class VendorType
    {
        [JsonProperty("VendorsTypeID")]
        public int? VendorTypeID { get; set; }
        [JsonProperty("VendorTypeList")]
        public string VendorTypeList { get; set; }
    }
}
