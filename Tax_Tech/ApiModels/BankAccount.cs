using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class BankAccount
    {
        public long? PaymentAccountID { get; set; }
        public long? PaymentAccountERPID { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAccountIBAN { get; set; }
        public string SwiftCode { get; set; }
        public string BranchAddress { get; set; }
        public long? BankID { get; set; }
        public long? EntityID { get; set; }
        public bool? IsActive { get; set; }
        public string BankName { get; set; }
    }
}
