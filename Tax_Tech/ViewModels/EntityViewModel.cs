using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ViewModels
{
    public class EntityViewModel
    {
        public long EntityId { get; set; }
        public string EntityTitle { get; set; }
        public bool IsActive { get; set; }
    }
}