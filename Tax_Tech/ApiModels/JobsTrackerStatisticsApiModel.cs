using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class JobsTrackerStatisticsApiModel
    {
        public long? JobID { get; set; }
        public byte LogTypeID { get; set; }
        public string LogTypeTitle { get; set; }
        public long? Count { get; set; }
    }
}
