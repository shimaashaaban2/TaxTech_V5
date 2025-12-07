using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.EReceiptApi;
using Tax_Tech.ApiModels.TaxUpdate;

namespace Tax_Tech.Repository
{
    public class JobTrackingRepository
    {
        public IEnumerable<JobTrackingModel> GetJobList()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetJobList");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<JobTrackingModel>>(content);
        }
        public JobTrackingModel GetJobListByJobId(int JobId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetJobStatusByID/{JobId}");
            var content = response.Content.ReadAsStringAsync().Result;


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<JobTrackingModel>(content);
        }
       
    
        public List<JobTrackingModel> GetJobListByDate(string From , string To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().
                GetResponse($"JobAPI/api/JobTracker/GetgetByDate/{From}/{To}");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<List<JobTrackingModel>>(content);
        }
        public JobLogPagedResult GetFailedJobList(int jobId, string logType, int pageNo, int pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/validation-failed/{jobId}/{logType}/{pageNo}/{pageSize}");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<JobLogPagedResult>(content);
        }


    }

}