using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class BranchTotals
    {
        public long? OwnerID { get; set; }
        public string BranchName { get; set; }
        public long? DocCount { get; set; }
        public double? TaxSum { get; set; }
        public double? TotalAmount { get; set; }
    }
}
