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
    public class EReceiptJobTrackingRepository
    {
        public IEnumerable<EreceiptJobTrackingModel> GetEReceiptJobList()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetEReceiptJobs");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EreceiptJobTrackingModel>>(content);
        }
        public IEnumerable<EreceiptJobTrackingModel> GetEReceiptJobByID(int jobID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetEReceiptJobs?jobID={jobID}");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EreceiptJobTrackingModel>>(content);
        }
        public IEnumerable<EreceiptJobTrackingModel> GetEReceiptJobByDate(string startDate,string endDate)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetEReceiptJobs?startDate={startDate}&endDate={endDate}");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EreceiptJobTrackingModel>>(content);
        }


        public EReceiptJobLogsModel GetEetEReceiptLogsByJobID(int jobId, string logType,int pageNumber=1,int pageSize =50)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetEReceiptLogsByJobID?jobId={jobId}&logType={logType}&pageNumber={pageNumber}&pageSize={pageSize}");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<EReceiptJobLogsModel>(content);
        }


    }
}