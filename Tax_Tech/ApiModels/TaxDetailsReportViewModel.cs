using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class TaxDetailsReportViewModel
    {
        public int? DocumentID { get; set; }
        public string UUID { get; set; }
        public string PersonalID { get; set; }
        public string VendorName { get; set; }
        public int? VendorID { get; set; }
        public int? OwnerID { get; set; }
        public string BranchName { get; set; }
        public string dateTimeIssued { get; set; }
        public string internalID { get; set; }
        public decimal? NetTotal { get; set; }
        public decimal? TaxTotals { get; set; }
        public decimal? Tbl02 { get; set; }
        public decimal? Ml02 { get; set; }
        public decimal? Ml04 { get; set; }
        public decimal? VAT { get; set; }
    }
}