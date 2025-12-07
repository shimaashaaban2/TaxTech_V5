using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax_Tech.ApiModels.EReceiptApi;

namespace Tax_Tech.Models
{
    public class EReceiptListViewModel
    {
       public  IEnumerable<ReceiptListModel> receiptLists { get; set; }
       public IEnumerable<ReceiptLinesTotals> ReceiptLinesTotals { get; set; }
    }
}