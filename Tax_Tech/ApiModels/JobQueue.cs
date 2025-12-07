using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class JobQueue
    {
        public long? SessionID { get; set; }
        public string JobTypeTitle { get; set; }
        public string CreationDate { get; set; }
        public string CreatorName { get; set; }
        public string FileName { get; set; }
        public string JobStatusTitle { get; set; }
        public long? NoOfRecords { get; set; }
        public string JobType { get; set; }
    }
}
