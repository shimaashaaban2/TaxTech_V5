using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class EReceiptJobLogsModel
    {
        public List<EReceiptLogs> Logs {  get; set; }
        public int totalCount { get; set; }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string logtype { get; set; }




    }
    public class EReceiptLogs
        {
            public int logID { get; set; }
            public string docID { get; set; }
            public string desc { get; set; }
            public string errorMessage { get; set; }
            public string logDate { get; set; }
            public string type { get; set; }
            public int jobID { get; set; }
        }
}

