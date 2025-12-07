using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class PriceAfterTaxModel
    {
        [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public Int64? itemID { get; set; }
        [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal? PriceAfterTax { get; set; }
        //[Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public short? By { get; set; }
    }

    public class PriceAfterTaxModelMaster
    {
        [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public Int64? itemID { get; set; }
        [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal? PriceAfterTax { get; set; }
       // [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal basicPrice { get; set; }
        //[Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal T2 { get; set; }
        //[Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal T3 { get; set; }
       // [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal DistributionExpenses { get; set; }
        //[Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal MI02 { get; set; }
       // [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public decimal MI04 { get; set; }
       // [Required(ErrorMessageResourceName = "EnterAllData", ErrorMessageResourceType = typeof(Resources.Resource))]

        public short? By { get; set; }
    }
}