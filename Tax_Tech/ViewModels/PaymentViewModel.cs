using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class PaymentViewModel
    {
        [Required(ErrorMessageResourceName = "AccountErpCodeCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        [RegularExpression("^[0-9]+$", ErrorMessageResourceName = "AccountERPCodeContainNumbersOnly", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string AccountErp { get; set; }

        [Required(ErrorMessageResourceName = "BankAccountNoCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string BankAccountNo { get; set; }

        [Required(ErrorMessageResourceName = "IBANNoCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string BankAccountIban { get; set; }

        [Required(ErrorMessageResourceName = "SwiftCodeCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string SwiftCode { get; set; }

        [Required(ErrorMessageResourceName = "BranchAddressCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string BranchAddress { get; set; }

        public string BankID { get; set; }

        public int? EntityID { get; set; }

        public long? ActionBy { get; set; }
        public int? PaymentAccountID { get; set; }
    }
}
