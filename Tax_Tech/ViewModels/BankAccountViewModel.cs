using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class BankAccountViewModel
    {
        public long PaymentAccountERPID { get; set; }
        public long BankAccountNo { get; set; }
        public string BankAccountIBAN { get; set; }
        public string SwiftCode { get; set; }
        public string BranchAddress { get; set; }
        public long BankID { get; set; }
        public long EntityID { get; set; }
        public long actionBy { get; set; }
        public long PaymentAccountID { get; set; }
    }
}
