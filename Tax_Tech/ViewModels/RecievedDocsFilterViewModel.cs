using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class RecievedDocsFilterViewModel
    {
        [Required(ErrorMessageResourceName = "plsSelectStartDate", ErrorMessageResourceType = typeof(Resources.Resource))]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectEndDate", ErrorMessageResourceType = typeof(Resources.Resource))]
        public DateTime? EndDate { get; set; }
        public int? ProcessStatusID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "SelectDocumentType")]
        public string DocumentTypetitle { get; set; }
    }
}
