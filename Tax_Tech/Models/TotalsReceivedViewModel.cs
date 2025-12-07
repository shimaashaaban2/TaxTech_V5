using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax_Tech.ApiModels.EReceiptApi;

namespace Tax_Tech.Models
{
    public class TotalsReceivedViewModel
    {
        public IEnumerable<TotalReceivedModel> totalReceiveds { get; set; }
        public IEnumerable<ProcessingSummeryModel> processingSummery { get; set;}
        public IEnumerable<ResultSummeryModel> resultSummeries { get; set; }
    }
}