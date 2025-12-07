using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels.InvoicingApi
{
    public class DocumentDetailsResult
    {
        [JsonProperty("submissionUUID")]
        public string SubmissionUUID { get; set; }
        [JsonProperty("dateTimeRecevied")]
        public string DateTimeRecevied { get; set; }
        [JsonProperty("validationResults")]
        public DocumentValidationResult ValidationResults { get; set; }
        public string UUID { get; set; }
    }
}
