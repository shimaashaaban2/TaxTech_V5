using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.TaxUpdate;

namespace Tax_Tech.Repository
{
    public class DocumentSourceRepo
    {
        public RootModel GetDocFilterBySource(string From , string To ,string source,int pageNo,int pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetDocFilterBySource/{From}/{To}/{source}/{pageNo}/{pageSize}");
              var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<RootModel>(content);
        }
    }
}