using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.EReceiptApi;
using Tax_Tech.ApiModels.ViewModels;
using Tax_Tech.Classes;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class LookupApiRepository
    {

        public IEnumerable<EReceiptSourceModel> GetEReceiptSource()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/Lookups/GetEReceiptSource", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<EReceiptSourceModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
          public IEnumerable<EReceiptStatusModel> GetEReceiptStatus()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/Lookups/GetEReceiptStatus", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<EReceiptStatusModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
          public IEnumerable<SubmitStatusModel> GetSubmitStatus()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/Lookups/GetSubmitStatus", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<SubmitStatusModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse GetActivityCodes(string entityId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ReadStringKey?EntityID={entityId}&ConfigKey=3", null);

                result.EnsureSuccessStatusCode();

                string content = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ApiResponse>(content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VendorType> GetVendorTypes()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetVendorsTypes", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<VendorType>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<UnitType> GetUnitTypes()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetUnitsList", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<UnitType>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<TaxType> GetTaxTypes()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetTaxTypeList", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<TaxType>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<Lup_CurrenyListView> GetCurrenyList()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/CurrenyList", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<Lup_CurrenyListView>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<Country> GetCountries(bool activeList)
        {
            try
            {
                HttpResponseMessage result = null;

                if (!activeList)
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetCountryList", null);
                }
                else
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetCountryActiveList", null);
                }

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<Country>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<ProcessStatus> GetProcessStatusList()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse("v2/WEB/Lookups/GetProcessStatusList?UseCase=1", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<ProcessStatus>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DocumentType> GetDocumentTypes()
        {
            try
            {
                return new List<DocumentType>
                {
                    new DocumentType
                    {
                        DocumentID = "1",
                        DocumentTypeName = "Invoice"
                    },
                    new DocumentType
                    {
                        DocumentID = "2",
                        DocumentTypeName = "Credit Note"
                    },
                    new DocumentType
                    {
                        DocumentID = "3",
                        DocumentTypeName = "Debit Note"
                    },
                  
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<MOFStatusListViewModel> MOFStatusList()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/MOFStatusList", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<MOFStatusListViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ReceivedVendorListViewModel> ReceivedVendorsList()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/RecivedAccountList", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ReceivedVendorListViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<Templates> GetTemplatesList(int? TemplateTypeID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetTemplateList?TemplateType={TemplateTypeID}", null);

            return new Tax_Tech.Helpers.ResponseResolver<Templates>().ResolveListResponse(result);
        }

        #region New GetTemplatesList
        public IEnumerable<Templates> NewGetTemplatesList(int? TemplateTypeID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetTemplateList?TemplateType={TemplateTypeID}", null);

            return new Tax_Tech.Helpers.ResponseResolver<Templates>().ResolveListResponse(result);
        }
        #endregion

        public ApiResponse GetTemplateURL(int? TemplateID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetTemplateURL?TemplateID={TemplateID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<Roles> GetRoleList()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetUserRoleList", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<Roles>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<UserApiModel> GetUsers()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse("v2/WEB/Permissions/UsersList", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<UserApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

    }
}
