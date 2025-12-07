using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels.InvoicingApi
{
    public class ErrorModel
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("details")]
        public List<ErrorModel> Details { get; set; }
    }
}
