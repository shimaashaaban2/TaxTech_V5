using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Classes;
using Tax_Tech.Exceptions;

namespace Tax_Tech.Repository.InvoicingApi
{
    public class DocumentsApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly Areas.Configuration.Repository.MOFConfigRepository _mOFConfigRepository;

        public DocumentsApiRepository(string entityId)
        {
            _mOFConfigRepository = new Areas.Configuration.Repository.MOFConfigRepository();

            var mofConfigResult = _mOFConfigRepository.GetMOFConfig(entityId);
            bool? isProduction = false;

            if(mofConfigResult != null)
            {
                isProduction = mofConfigResult.EnviromentBool;
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(isProduction == true ? PublicConfig.Production_API_URL : PublicConfig.PreProduction_API_URL)
            };
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en");
        }

        public DocumentDetailsResult GetDocumentDetails(string uuid, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var httpResponse = _httpClient.GetAsync($"v1/documents/{uuid}/details").Result;
            var content = httpResponse.Content.ReadAsStringAsync().Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<DocumentDetailsResult>(content);
            }

            if(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new UnauthorizedAccessException($"{content}");
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new NotAuthorizedException(Resources.Resource.Unauthorized);
            else
                throw new HttpRequestException($"{content}");
        }

        public string CancelDocument(string uuid, string reason, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var httpResponse = _httpClient.PutAsJsonAsync($"v1.0/documents/state/{uuid}/state", new {
                status = "cancelled",
                reason
            }).Result;

            var content = httpResponse.Content.ReadAsStringAsync().Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                return content;
            }

            if(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new UnauthorizedAccessException($"{content}");
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new NotAuthorizedException(Resources.Resource.Unauthorized);
            else
                throw new HttpRequestException($"{content}");
        }

        public byte[] GetDocumentAsPdf(string uuid, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
            var httpResponse = _httpClient.GetAsync($"v1/documents/{uuid}/pdf").Result;


            if(httpResponse.IsSuccessStatusCode)
            {
                var binaryContent = httpResponse.Content.ReadAsByteArrayAsync().Result;
                return binaryContent;
            }

            var content = httpResponse.Content.ReadAsStringAsync().Result;

            if(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new UnauthorizedAccessException($"{content}");
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new NotAuthorizedException(Resources.Resource.Unauthorized);
            else
                throw new HttpRequestException($"{content}");
        }

        public RecentDocumentResult GetRecentDocs(string accessToken, int? pageNo, int? pageSize)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
            var httpResponse = _httpClient.GetAsync($"v1/documents/recent?PageSize={pageSize}&PageNo={pageNo}").Result;
            var content = httpResponse.Content.ReadAsStringAsync().Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<RecentDocumentResult>(content);
            }

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new UnauthorizedAccessException($"{content}");
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new NotAuthorizedException(Resources.Resource.Unauthorized);
            else
                throw new HttpRequestException($"{content}");
        }
        
    }
}
