using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class DocumentTotalReport
    {
        public long? ProcessStatusID { get; set; }
        public string ProcessStatusTitle { get; set; }
        public long? DocCount { get; set; }
        public double? TaxTotals { get; set; }
        public double? TotalAmount { get; set; }
    }
}
