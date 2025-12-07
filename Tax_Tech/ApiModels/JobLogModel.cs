using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class JobLogPagedResult
    {
        public List<JobLogModel> Logs { get; set; } 
        public int TotalCount { get; set; }

        public string logType { get; set; }
    }
    public class JobLogModel
    {

        public long LogID { get; set; }
        public long JobID { get; set; }
        public DateTime OperationDate { get; set; }
        public string VendorName { get; set; }
        public string InternalID { get; set; }
        public string Details { get; set; }

    }
}


