using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax_Tech.ApiModels;

namespace Tax_Tech.ViewModels
{
    public class SubmissionLogsViewModel
    {
        public CustomResponse CustomeRespons { get; set; }
        public long LogID { get; set; }
        public string request { get; set; }
        public string Response { get; set; }
        public string DocID { get; set; }
        public System.DateTime RequestDate { get; set; }
        public bool IsSucceeded { get; set; }
        public string SignCode { get; set; }
        public string internalID { get; set; }

        public byte SignStatus { get; set; }

    }
}