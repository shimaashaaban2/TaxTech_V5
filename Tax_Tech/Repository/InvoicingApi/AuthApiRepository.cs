using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Classes;

namespace Tax_Tech.Repository.InvoicingApi
{
    public class AuthApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly Areas.Configuration.Repository.MOFConfigRepository _mOFConfigRepository;
        private string clientID = string.Empty;
        private string clientSecret = string.Empty;

        public AuthApiRepository(string entityId)
        {
            _mOFConfigRepository = new Areas.Configuration.Repository.MOFConfigRepository();
            var mofConfigResult = _mOFConfigRepository.GetMOFConfig(entityId);
            bool? isProduction = false;

            if(mofConfigResult != null)
            {
                isProduction = mofConfigResult.EnviromentBool;
                clientID = mofConfigResult.ClientID;
                clientSecret = mofConfigResult.ClientSecret;
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(isProduction == true ? PublicConfig.ProductionTokenAPIPath : PublicConfig.PreProductionTokenAPIPath)
            };
        }

        public TaxPayerLoginResult LoginAsTaxPayer()
        {
            var postContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", PublicConfig.GrantType),
                new KeyValuePair<string, string>("client_id", clientID),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", PublicConfig.Scope)
            });

            var httpResponse = _httpClient.PostAsync("", postContent).Result;
            var content = httpResponse.Content.ReadAsStringAsync().Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TaxPayerLoginResult>(content);
            }

            if(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new UnauthorizedAccessException(content);
            }
            
            throw new HttpRequestException($"{content}");
        }
    }
}
