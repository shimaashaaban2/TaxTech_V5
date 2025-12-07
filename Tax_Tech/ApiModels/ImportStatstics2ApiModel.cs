using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class ImportStatstics2ApiModel
    {
        public long? TotalSignUp { get; set; }
        public long? SignedInvoices { get; set; }
        public long? SubmittedSueccs { get; set; }
        public long? ESignedFaliuer { get; set; }
        public long? SignupFaliuer { get; set; }
        public long? SubmittedFaliuer { get; set; }
    }
}
