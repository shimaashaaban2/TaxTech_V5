using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{

    
    public class RootModel
    {
        [JsonProperty("docs")]
        public List<DocumentSourceModel> docs { get; set; }

        [JsonProperty("totalCount")]
        public int totalCount { get; set; }

       
    }


    public class DocumentSourceModel
    {
        public int? DocumentID { get; set; }
        public int? ProcessStatusID { get; set; }
        public int? Uuid { get; set; }   // int? بدل int مع default 0
        public string VendorName { get; set; }   // string طبيعي nullable في .NET Framework
        public int? OwnerID { get; set; }
        public string DocumentType { get; set; }
        public DateTime? DateTimeIssued { get; set; }   // أنسب من int لو التاريخ ISO string
        public int? TaxpayerActivityCode { get; set; }
        public string InternalID { get; set; }   // غالباً ده مش int، جاي من JSON زي "B0000000-…"
        public int? EntityID { get; set; }
        public string EntityTitle { get; set; }
        public int? ActionBy { get; set; }
        public int? MofRejectStatus { get; set; }
        public int? JobID { get; set; }
        public string Source { get; set; }
        public decimal NetTotal { get; set; }
        public decimal TaxTotals { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
