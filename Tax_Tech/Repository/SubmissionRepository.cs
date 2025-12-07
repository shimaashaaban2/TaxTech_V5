using Newtonsoft.Json;
using System;
using System.Collections.Generic; 
using System.Net.Http; 
using Tax_Tech.ViewModels;

namespace Tax_Tech.Repository
{
    public class SubmissionRepository
    {
        
        private static SubmissionRepository _instance;
        public static SubmissionRepository Get()
        {
            if (_instance == null)
            {
                _instance = new SubmissionRepository();
            }
            return _instance;
        }
        public IEnumerable<SubmissionLogsViewModel> GetSubmissionLogByDate(long EntityID, DateTime from, DateTime to, byte Status)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Log/GetSubmissionLogByDate?EntityID={EntityID}&from={from}&to={to}&Status={Status}", null);
                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<SubmissionLogsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<SubmissionLogsViewModel> GetSubmissionLogByInternalID(string InternalID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Log/GetSubmissionLogByInternalID?InternalID={InternalID}", null);
                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<SubmissionLogsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<SubmissionLogsViewModel> GetSubmissionLogByDocID(long DocID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Log/GetSubmissionLogByDocID?DocID={DocID}", null);
                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<SubmissionLogsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}