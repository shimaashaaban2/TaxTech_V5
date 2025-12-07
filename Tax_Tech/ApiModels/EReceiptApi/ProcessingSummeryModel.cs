using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class ProcessingSummeryModel
    {
        public int CountOfNormalizFail { get; set; }
        public int CountOfSubmited { get; set; }
        public int CountOfPending { get; set; }
        public int CountOfFailToSubmit { get; set; }
    }
}

