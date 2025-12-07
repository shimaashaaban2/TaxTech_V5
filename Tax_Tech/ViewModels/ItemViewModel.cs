using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class ItemViewModel
    {
        [Required(ErrorMessageResourceName = "ItemNameCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ItemName { get; set; }
        public string UnitTitleAR { get; set; }
        public string UnitTitleEN { get; set; }
        public string TaxPortalUnitID { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectItemType", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ItemType { get; set; }

        [Required(ErrorMessageResourceName = "ItemERPCodeCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ItemERPID { get; set; }
        
        [Required(ErrorMessageResourceName = "ItemCodeCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectUnitType", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ItemUnitID { get; set; }
        [Required(ErrorMessageResourceName = "PlsSelectEntity", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int? EntityID { get; set; }
        public string EntityTitle { get; set; }
        public long? ActionBy { get; set; }
        public string ItemSerial { get; set; }
        public bool? IsActive { get; set; }
        public int? TotalCount { get; set; }
        public decimal? PriceAfterTax { get; set; }
    }
}
