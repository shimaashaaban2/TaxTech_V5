using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{

    public class ExportItems 
    {
        public AllItemsNotExist[] AllItemsNotExist { get; set; }
    }
    public class AllItemsNotExist
    {
     
        public string internalCode { get; set; }
        public string description { get; set; }
    }
}