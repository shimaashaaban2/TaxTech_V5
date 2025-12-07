using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class ObjectCustomeRespons
    {
        public string ResponseID { get; set; }
        public string ResponseMsg { get; set; }
        public List<string> ResponseList { get; set; }
    }
}