using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class ItemGroupsListModel
    {
        public int ItemGroupID { get; set; }
        public string ItemGroupTitle { get; set; }
        public bool IsActive { get; set; }
    }
}