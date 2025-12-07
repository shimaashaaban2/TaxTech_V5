using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class MOFStatusListViewModel
    {
        public long SubmitStstusID { get; set; }
        public string SubmitStatusTitle { get; set; }
        public long OrderBy { get; set; }
    }
}