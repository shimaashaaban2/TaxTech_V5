using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class BranchViewModel
    {
        [Required(ErrorMessageResourceName = "BranchTypeCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public long? BranchAPIType { get; set; }

        [Required(ErrorMessageResourceName = "BranchNameCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string BranchName { get; set; }

        [Required(ErrorMessageResourceName = "RegInfoCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string PersonalID { get; set; }

        [Required(ErrorMessageResourceName = "BranchCodeCannotBeKeptEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string BranchCode { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectCountry", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string CountryID { get; set; }

        public bool? IsActive { get; set; }

        [JsonProperty("governate")]
        [Required(ErrorMessageResourceName = "GovernateCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Governate { get; set; }
        
        [JsonProperty("regionCity")]
        [Required(ErrorMessageResourceName = "CityCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string RegionCity { get; set; }

        [JsonProperty("street")]
        [Required(ErrorMessageResourceName = "StreetCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Street { get; set; }

        [JsonProperty("buildingNumber")]
        [Required(ErrorMessageResourceName = "BuildingNoCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string BuildingNo { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
        
        [JsonProperty("floor")]
        public string Floor { get; set; }

        [JsonProperty("flat")]
        public string Flat { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }
        
        [JsonProperty("additionalInformation")]
        public string AdditionalInformation { get; set; }
        public string CountryName { get; set; }
        
        [Required(ErrorMessageResourceName = "ErpCodeCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ERPInternalID { get; set; }
        [Required(ErrorMessageResourceName = "PlsSelectEntity", ErrorMessageResourceType = typeof(Resources.Resource))]
        public long? EntityID { get; set; }
        public long? ActionBy { get; set; }
        public long? BranchID { get; set; }
         public string EntityTitle { get; set; }
    }
}
