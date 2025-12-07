using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class DocumentCountTotals
    {
        public int TotalInvoices { get; set; }
        public int AcceptedInvoices {get; set;}
        public int MofRejectedInvoices {get; set;}
        public int MiddlewareRejectedInvoices {get; set;}
    }
}
