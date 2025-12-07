using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class CreateInvoiceHead
    {
        [Required(ErrorMessageResourceName = "InvoiceInternalIDErr", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string InvoiceInternalID { get; set; }

        [Required(ErrorMessageResourceName = "InvocieIssueDateErr", ErrorMessageResourceType = typeof(Resources.Resource))]
        [DateNotInFuture(ErrorMessageResourceName = "DocumentIssueDateCannotBeInTheFuture", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string InvoiceIssueDate { get; set; }

        public string ActivityCode { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectBranch", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string OwnerID { get; set; }

        //[Required(ErrorMessageResourceName = "PlsSelectVendor", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string VendorID { get; set; }
        public int EntityId { get; set; }
        public long ActionBy { get; set; }
        public string PurchaseOrderReference { get; set; }
        public string PurchaseOrderDescription { get; set; }
        public string SalesOrderReference { get; set; }
        public string SalesOrderDescription { get; set; }
        public string ProformaInvoiceNumber { get; set; }
        public string VendorName { get; set; }
    }
}
