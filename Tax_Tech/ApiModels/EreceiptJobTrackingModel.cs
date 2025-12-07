using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class EreceiptJobTrackingModel
    {
        public int jobID { get; set; }
        public int totalCount { get; set; }
        public int insertedHeader { get; set; }
        public int insertedLines { get; set; }
        public int notInsertedHeaders { get; set; }
        public int notInsertedLines { get; set; }
        public int processedCount { get; set; }
        public string status { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
