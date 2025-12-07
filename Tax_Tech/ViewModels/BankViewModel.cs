using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class BankViewModel
    {
        public long? BankID { get; set; }
        [Required]
        [Display(Name = "Bankname", ResourceType = typeof(Resources.Resource))]
        public string BankName { get; set; }
        public bool? Status { get; set; }
    }
}
