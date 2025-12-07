using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.Areas.Configuration.ViewModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class LogRepository
    {
       
        private static LogRepository _instance;
        public static LogRepository Get()
        {
            if (_instance == null)
            {
                _instance = new LogRepository();
            }
            return _instance;
        }
        public IEnumerable<LogsViewModel> GetMOFLogByDate(long EntityID, DateTime from, DateTime to)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Log/GetMOFLogByDate?EntityID={EntityID}&from={from}&to={to}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<LogsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<LogsViewModel> GetMOFLogByInternalID(string InternalID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Log/GetMOFLogByInternalID?InternalID={InternalID}", null);
                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<LogsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}