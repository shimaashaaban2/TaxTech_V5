using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels.InvoicingApi
{
    public class ValidationErrModel
    {
        [JsonProperty("error")]
        public ErrorModel Error { get; set; }
    }
}
