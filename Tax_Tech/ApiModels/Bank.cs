using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class Bank
    {
        public long? BankID { get; set; }
        public string BankName { get; set; }
        public bool? IsActive { get; set; }
        public long? ActionBy { get; set; }
    }
}
