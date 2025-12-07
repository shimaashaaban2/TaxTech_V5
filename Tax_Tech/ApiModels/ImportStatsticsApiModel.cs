using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class ImportStatsticsApiModel
    {
        public long? TotalRecords { get; set; }
        public long? ImportSuccess { get; set; }
        public long? ImportInProgress { get; set; }
        public long? ImportFail { get; set; }
    }
}
