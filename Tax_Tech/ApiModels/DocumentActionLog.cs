using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class DocumentActionLog
    {
        public long? StepID { get; set; }
        public long? DocumentId { get; set; }
        public long? ProcessID { get; set; }
        public string ProcessStatusTitle { get; set; }
        public string ProcessDate { get; set; }
        public string ProcessMessage { get; set; }
    }
}
