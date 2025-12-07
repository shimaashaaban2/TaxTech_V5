using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Repository
{
    public class VendorsApiRepositoryV2
    {
       
        public string InsertVendorV2(VendorViewModel model)
        {
            var payload = new
            {
                model.VendorAPIType,
                model.VendorName,
                model.PersonalID,
                model.CountryID,
                model.Governate,
                model.RegionCity,
                model.Street,
                model.BuildingNo,
                model.PostalCode,
                model.Floor,
                model.Flat,
                model.Landmark,
                model.AdditionalInformation,
                model.PhoneNumber,
                model.ERPInternalID,
                model.EntityID,
                model.ActionBy,
                model.TaxExmptedCount,
                model.IsTaxExempted
            };

            // grab the singleton helper
            var api = ApiTokenRepository.GetAPI();

            // use its Client property with the standard HttpClient extensions
            var response = api.PostResponse(
                "testApi/api/BasicVendors/InsertVendor", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsStringAsync().Result;
                throw new Exception(error);
            }

            var json = response.Content.ReadAsStringAsync().Result;
            return json;
        }
        public string UpdateVendorV2(VendorViewModel model)
        {

            var payload = new
            {
                model.VendorAPIType,
                model.VendorName,
                model.PersonalID,
                model.CountryID,
                model.Governate,
                model.RegionCity,
                model.Street,
                model.BuildingNo,
                model.PostalCode,
                model.Floor,
                model.Flat,
                model.Landmark,
                model.AdditionalInformation,
                model.PhoneNumber,
                model.ERPInternalID,
                model.EntityID,
                model.ActionBy,
                model.VendorID,
                model.TaxExmptedCount,
                model.IsTaxExempted
            };

            // grab the singleton helper
            var api = ApiTokenRepository.GetAPI();

            // use its Client property with the standard HttpClient extensions
            var response = api.PostResponse(
                "testApi/api/BasicVendors/UpdateVendor", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsStringAsync().Result;
                throw new Exception(error);
            }

            var json = response.Content.ReadAsStringAsync().Result;
            return json;
           
        }
    }
}

