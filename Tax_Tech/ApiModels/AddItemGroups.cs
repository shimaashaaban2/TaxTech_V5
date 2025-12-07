using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class AddItemGroups
    {
        public long ItemGroupID { get; set; }
        public long ItemID { get; set; }
        public long UserID { get; set; }
       
    }
}