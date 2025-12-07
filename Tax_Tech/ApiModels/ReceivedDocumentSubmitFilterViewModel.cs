using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class ReceivedDocumentSubmitFilterViewModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool docTypeOption { get; set; }
        public byte? mDocTypeID { get; set; }
        public bool mOFStatusOption { get; set; }
        public string mOStatus { get; set; }
        public bool accountOption { get; set; }
        public string accountName { get; set; }
    }
}