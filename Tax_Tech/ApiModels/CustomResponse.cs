using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class CustomResponse
    {
        [JsonProperty("Response ID")]
        public string ResponseID { get; set; }
        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("Response MSG")]
        public string ResponseMsg { get; set; }

        [JsonProperty("Document DBID")]
        public string DocumentID { get; set; }

        public double? ItemTotalTax { get; set; }

        public string UUID { get; set; }

        [JsonProperty("Linked Invoices")]
        public string LinkedInvoices { get; set; }
        public string URL { get; set; }
        public string Hours { get; set; }
        public string FileLink { get; set; }
        public string CountOfAll { get; set; }
        public string EntityID { get; set; }
        public string ERPInternalID { get; set; }
        public string PersonalID { get; set; }
        public string CodeExists { get; set; }
        public string HasPermission { get; set; }
    }
}
