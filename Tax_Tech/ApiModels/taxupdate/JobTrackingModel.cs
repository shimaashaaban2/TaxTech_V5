using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.TaxUpdate
{
    public class JobTrackingModel
    {
        public int jobID { get; set; }
        public int totalCount { get; set; }
        public int insertedDocuments { get; set; }
        public string status { get; set; }
        public int notInsertedLines { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public decimal progressPercent { get; set; }
        public int processedCount { get; set; }
        public int insertedLines { get; set; }
        public int notInsertedDocuments { get; set; }
    }
}
