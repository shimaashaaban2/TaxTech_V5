using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.Areas.Configuration.ViewModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Repository
{
    public class JobQueueRepositoryV2
    {
        public string GetCloseJobV2Async(JobQueueModelV2 jobQueue)
        {
            var payload = new
            {
                jobQueue.SessionId,
                jobQueue.JobStatus,
                jobQueue.CreatedBy
            };

            // grab the singleton helper
            var api = ApiTokenRepository.GetAPI();

            // use its Client property with the standard HttpClient extensions
            var response = api.PostResponse(
                "testApi/api/JobQueue/change-status", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error =  response.Content.ReadAsStringAsync().Result;
                throw new Exception(error);
            }

            var json =  response.Content.ReadAsStringAsync().Result;
            return json;
        }
    }
}

//HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"testApi/api/Invoice/generate", new { requestDto.DocumentId, requestDto.PhoneNumber });
//var content = response.Content.ReadAsStringAsync().Result;

//if (!response.IsSuccessStatusCode)
//{
//    throw new Exception(content);
//}
//return JsonConvert.DeserializeObject<SendFileModel>(content);