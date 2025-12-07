using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels.InvoicingApi
{
    public class MetaData
    {
        [JsonProperty("totalPages")]
        public int? TotalPages { get; set; }

        [JsonProperty("totalCount")]
        public int? TotalCount { get; set; }
    }
}
