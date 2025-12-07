using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class ActivityCode
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        public string Desc_en { get; set; }
        public string Desc_ar { get; set; }
    }
}
