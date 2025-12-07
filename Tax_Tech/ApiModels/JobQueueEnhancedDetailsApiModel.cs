using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class JobQueueEnhancedDetailsApiModel
    {
        public long RecordID { get; set; }
        public string InternalID { get; set; }
        public string LogCode { get; set; }
        public string Error { get; set; }
        public string ErrorDetails { get; set; }
    }
}
