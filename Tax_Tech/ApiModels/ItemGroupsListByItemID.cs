using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class ItemGroupsListByItemID
    {
        public string ItemGroupTitle { get; set; }
        public long SerialRecored { get; set; }
        public int ItemGroupID { get; set; }
        public long ItemSerial { get; set; }
        public string ItemName { get; set; }
    }
}