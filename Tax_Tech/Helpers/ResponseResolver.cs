using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;

namespace Tax_Tech.Helpers
{
    public class ResponseResolver<T>
    {
        public IEnumerable<T> ResolveListResponse(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content.ReadAsStringAsync().Result);
        }

        public T ResolveObjectResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            if(typeof(T).Name == nameof(ApiResponse))
            {
                var content = JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
                if (content.CustomeRespons.ResponseID != "1")
                    throw new Exception(content.CustomeRespons.ResponseMsg);
            }

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
