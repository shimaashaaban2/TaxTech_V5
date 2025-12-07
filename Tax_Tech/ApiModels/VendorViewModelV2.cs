using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class VendorViewModelV2
    {
        [Required(ErrorMessageResourceName = "PlsSelectVendorType", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int? VendorAPIType { get; set; }

        [Required(ErrorMessageResourceName = "VendorNameCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string VendorName { get; set; }

        [Required(ErrorMessageResourceName = "RegInfoCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string PersonalID { get; set; }

        [Required(ErrorMessageResourceName = "PlsSelectCountry", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string CountryID { get; set; }
        public string CountryName { get; set; }
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
        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceName = "ErpCodeCannotbeEmpty", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string ERPInternalID { get; set; }
        [Required(ErrorMessageResourceName = "PlsSelectEntity", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int? EntityID { get; set; }
        public string EntityTitle { get; set; }
        public long? ActionBy { get; set; }
        public int? VendorID { get; set; }

        public bool? IsTaxExempted { get; set; }
        public int? TaxExmptedCount { get; set; }
        public int? TotalCount { get; set; }
    }
}