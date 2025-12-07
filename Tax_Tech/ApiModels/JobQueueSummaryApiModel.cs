using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class JobQueueSummaryApiModel
    {
        public long? SOrder { get; set; }
        public string STitle { get; set; }
        public long? SCount { get; set; }
        public long? JobID { get; set; }
        public long? TotalSignUp { get; set; }
        public long? SignedInvoices { get; set; }
        public long? SubmittedSueccs { get; set; }
        public long? SubmittedFaliuer { get; set; }
        public long? TotalRecords { get; set; }
        public long? ImportSuccess { get; set; }
        public long? ImportInProgress { get; set; }
        public long? ImportFail { get; set; }
        public long? JobTypeID { get; set; }
        public DateTime? CreationDate { get; set; }
        public string StatusTitle { get; set; }
    }
}
