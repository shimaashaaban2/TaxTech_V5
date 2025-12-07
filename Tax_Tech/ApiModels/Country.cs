using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class Country
    {
        public long? CountryID { get; set; }
        public string CountryName { get; set; }
        public string ISO3 { get; set; }
        public string ISO2 { get; set; }
        public string PhoneCode { get; set; }
        public string Capital { get; set; }
        public string Currency { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public bool? IsActive { get; set; }
    }
}
