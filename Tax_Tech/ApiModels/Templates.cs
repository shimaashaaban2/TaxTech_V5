using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class Templates
    {

        public int? TemplateID { get; set; }
        public int? TemplateType { get; set; }
        public string TemplateName { get; set; }
        public string TemplateURL { get; set; }
        public bool? IsActive { get; set; }

    }
}