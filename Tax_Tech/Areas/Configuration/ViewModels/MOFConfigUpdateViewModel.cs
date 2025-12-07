using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.Areas.Configuration.ViewModels
{
    public class MOFConfigUpdateViewModel
    {

        public long EntityID { get; set; }
        public string configKeyTitle { get; set; }
        public string configKeyStringValue { get; set; }
        public bool? configKeyBoolValue { get; set; }
        public bool IsEncrypted { get; set; }
        public string key { get; set; }
    }
}