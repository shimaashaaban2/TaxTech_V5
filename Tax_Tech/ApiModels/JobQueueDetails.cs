using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class JobQueueDetails
    {
        public long? RecordID { get; set; }
        public long? JobID { get; set; }
        public string DocID { get; set; }
        public string DocDate { get; set; }
        public long? ExcelRowIndex { get; set; }
        public string ExcelSheetName { get; set; }
        public string ImportResult { get; set; }
    }
}
