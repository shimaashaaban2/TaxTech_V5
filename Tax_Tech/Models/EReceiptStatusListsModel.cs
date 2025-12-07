using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax_Tech.ApiModels.EReceiptApi;

namespace Tax_Tech.Models
{
    public class EReceiptStatusListsModel
    {
        public IEnumerable<EReceiptSourceModel> eReceiptSources {  get; set; } 
        public IEnumerable<EReceiptStatusModel> eReceiptStatuses {  get; set; } 
        public IEnumerable<SubmitStatusModel> submitStatuses {  get; set; } 
        public IEnumerable<EReceiptFullLogsModel>eReceiptFullLogs { get; set; }
    }
}