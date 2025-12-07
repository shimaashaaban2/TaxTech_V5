using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.Areas.Configuration.ViewModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class MOFConfigRepository
    {


        public MOFConfigViewModel GetMOFConfig(string entityId)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetEntityMOFConfig?EntityID={entityId}", null);

            return new Tax_Tech.Helpers.ResponseResolver<MOFConfigViewModel>().ResolveObjectResponse(result);
        }

        public ApiResponse UpdateMOFConfig(MOFConfigUpdateViewModel model)
        {
            try
            {
                //ConfigKey, long , string , string , bool? 
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/InsertStringKey?EntityID={model.EntityID}&" +
                    $"ConfigKeyTitle={model.configKeyTitle}&ConfigKeyStringValue={model.configKeyStringValue}&ConfigKeyBoolValue={model.configKeyBoolValue}&IsEncrypted={model.IsEncrypted}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ApiResponse UpdateMOFConfigKeys(MOFConfigUpdateViewModel model)
        {
            try
            {
                //ConfigKey, long , string , string , bool? 
                HttpResponseMessage result = ApiTokenRepository.GetAPIWithHeader(model.key).PostResponse($"v2/WEB/Config/InsertKeys?EntityID={model.EntityID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}