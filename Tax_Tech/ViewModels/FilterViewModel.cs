using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class FilterViewModel
    {
        [Required(ErrorMessageResourceName = "plsSelectStartDate", ErrorMessageResourceType = typeof(Resources.Resource))]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectEndDate", ErrorMessageResourceType = typeof(Resources.Resource))]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "SelectDocumentType")]
        public int DocumentType { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PlsSelectDocStatus")]
        public int? ProcessStatusID { get; set; }
        public int? PageNo { get; set; } = 1;
        public int? PageSize { get; set; } = 100;
        public long? EntityID { get; set; }
    }
}
