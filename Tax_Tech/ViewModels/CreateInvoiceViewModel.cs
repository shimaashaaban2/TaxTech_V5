using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;

namespace Tax_Tech.ViewModels
{
    public class CreateInvoiceViewModel
    {
        public IEnumerable<ActivityCode> ActivityCodes { get; set; }
    }
}
