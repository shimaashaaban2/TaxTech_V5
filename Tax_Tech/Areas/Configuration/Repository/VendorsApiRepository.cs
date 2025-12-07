using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class VendorsApiRepository
    {
        public IEnumerable<VendorViewModel> GetAllVendors()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse("v2/WEB/Config/GetVendorList", new { });

            return new Tax_Tech.Helpers.ResponseResolver<VendorViewModel>().ResolveListResponse(response);
        }

        public ApiResponse InsertVendor(VendorViewModel model)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/VendorInsert?VendorAPIType={model.VendorAPIType}&VendorName={model.VendorName}&PersonalID={model.PersonalID}&CountryID={model.CountryID}&governate={model.Governate}&regionCity={model.RegionCity}&street={model.Street}&buildingNumber={model.BuildingNo}&postalCode={model.PostalCode}&floor={model.Floor}&flat={model.Flat}&landmark={model.Landmark}&additionalInformation={model.AdditionalInformation}&ERPInternalID={model.ERPInternalID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}&IsTaxExempted={model.IsTaxExempted}", null);

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        public ApiResponse UpdateVendor(VendorViewModel model)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/VendorUpdate?VendorAPIType={model.VendorAPIType}&VendorName={model.VendorName}&PersonalID={model.PersonalID}&CountryID={model.CountryID}&governate={model.Governate}&regionCity={model.RegionCity}&street={model.Street}&buildingNumber={model.BuildingNo}&postalCode={model.PostalCode}&floor={model.Floor}&flat={model.Flat}&landmark={model.Landmark}&additionalInformation={model.AdditionalInformation}&ERPInternalID={model.ERPInternalID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}&VendorID={model.VendorID}&IsTaxExempted={model.IsTaxExempted}", null);

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        public IEnumerable<VendorViewModel> GetVendors(bool activeList)
        {

            HttpResponseMessage result = null;

            if (activeList)
            {
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorActiveList", null);
            }
            else
            {
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorList", null);
            }

            return new Tax_Tech.Helpers.ResponseResolver<VendorViewModel>().ResolveListResponse(result);
        }
        public IEnumerable<VendorViewModel> GetVendorsByEntity(int? entityId)
        {
            HttpResponseMessage result = null;

            if (entityId != null)
            {
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorToExport?EntityID={entityId}", null);
            }
            else
            {
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorList", null);
            }

            return new Tax_Tech.Helpers.ResponseResolver<VendorViewModel>().ResolveListResponse(result);
        }
        public ApiResponse GetVendorsCount()
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorCountOfAll", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VendorViewModel> GetVendorsByFilter(string Filter)
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorListByFilter?Filter=" + Filter, null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VendorViewModel> GetVendorsByFilter(string filter, int? pageNo, int? pageSize)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorListByFilterWithPagination?filter={filter}&pageNo={pageNo}&pageSize={pageSize}", new { });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(result.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<VendorViewModel> GetVendorsByFilterAndEntityId(long? entityId, string filter, int? pageNo, int? pageSize)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorListByFilterAndEntityIDWithPagination?entityId={entityId}&filter={filter}&pageNo={pageNo}&pageSize={pageSize}", new { });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(result.Content.ReadAsStringAsync().Result);
        }

        public ApiResponse ChangeStatus(bool status, int vendorId, long actionBy)
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/SetVendorActive?VendorID={vendorId}&IsActive={status}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VendorViewModel> GetVendorById(int vendorId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorByID?VendorID={vendorId}", null);

                result.EnsureSuccessStatusCode();

                var res = JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(result.Content.ReadAsStringAsync().Result);

                return res;

            }
            catch (Exception)
            {
                return null;
            }
        }

        #region GetVendorById
        public IEnumerable<VendorViewModel> NewGetVendorById(int vendorId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorByID?VendorID={vendorId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetVendorByName
        public IEnumerable<VendorViewModel> GetVendorByName(string vendorName)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorByName?Name={vendorName}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        public bool? CheckVendorErpCodeExistForInsert(long? entityId, string eRP)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/VendorsInsertCheckERPExists?entityId={entityId}&erp={eRP}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<bool?>(response.Content.ReadAsStringAsync().Result);
        }

        public bool? CheckVendorErpCodeExistForUpdate(long? entityId, string eRP, long? vendorId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/VendorsUpdateCheckERPExists?entityId={entityId}&erp={eRP}&vendorId={vendorId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<bool?>(response.Content.ReadAsStringAsync().Result);
        }

        #region By Entity ID
        public IEnumerable<VendorViewModel> GetVendorsByEntityId(long? entityId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorListByEntityID?entityId={entityId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public ApiResponse GetVendorsCountByEntityId(long? entityId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorCountByEntityID?entityId={entityId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<VendorViewModel> GetVendorByFilterAndEntityId(string filter, int? entityId)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetVendorListByFilterAndEntityID?filter={filter}&entityId={entityId}", null);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<VendorViewModel>>(result.Content.ReadAsStringAsync().Result);
        }
        #endregion

        #region Exempted Taxes
        public ApiResponse CreateTaxExemption(long? vendorId, byte? taxTypeID, bool? isActive)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/TaxExmpted/Insert?vendorID={vendorId}&taxTypeID={taxTypeID}&isActive={isActive}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        public ApiResponse RemoveTaxExemption(long? id)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/TaxExmpted/Remove?id={id}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<VendorTaxViewModel> GetTaxExemptionList(long? vendorId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/TaxExmpted/List?vendorId={vendorId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<VendorTaxViewModel>>(response.Content.ReadAsStringAsync().Result);
        }
        #endregion
    }
}
