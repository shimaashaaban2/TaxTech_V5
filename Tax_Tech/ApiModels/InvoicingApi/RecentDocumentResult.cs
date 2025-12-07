using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels.InvoicingApi
{
    public class RecentDocumentResult
    {
        [JsonProperty("result")]
        public IEnumerable<RecentDocument> Result { get; set; }

        [JsonProperty("metadata")]
        public MetaData MetaData { get; set; }
    }
}
